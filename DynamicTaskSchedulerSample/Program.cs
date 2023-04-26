using Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using DynamicTaskSchedulerSample;

List<SchedulerSettings>? settings = null;

var builder = Host.CreateDefaultBuilder().ConfigureServices((cxt, services) =>
{
    services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
    });

    //For gracefull shutdown
    services.AddQuartzHostedService(opt =>
    {
        opt.WaitForJobsToComplete = true;
    });

    settings = cxt.Configuration.GetSection(SchedulerSettings.SectionName).Get<List<SchedulerSettings>>();
    //Add it if you want to use settings in DI
    //services.Configure<Settings>(cxt.Configuration.GetSection(SchedulerSettings.SectionName));

}).Build();

var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();


if (settings != null)
{
    foreach (var item in settings)
    {
        // Define the job class with reflections
        if (Type.GetType(item.Method) is object)
        {
            //Create a job with the Type of namespaces
            var jobs = JobBuilder.Create(jobType: Type.GetType(item.Method))
           .WithIdentity(item.Name, item.GroupName)
           .Build();

            // Trigger the job to run now, and then every 40 seconds
            var triggers = TriggerBuilder.Create()
            .WithIdentity(item.Name, item.GroupName)
            .StartNow()
            .UsingJobData("Data", item.Data)
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(item.Interval)
            .RepeatForever())
            .Build();

            await scheduler.ScheduleJob(jobs, triggers);
        }
        else
        {
            throw new ArgumentException("'Method' parameter is not a valid an object");
        }
    }
}
else
{
    throw new ArgumentException("Can't read settings");
}

// will block until the last running job completes
await builder.RunAsync();
