using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQuarzJobs.Jobs
{
    internal class JobTwo : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var data = context.MergedJobDataMap.GetString("Data");

                //Do some stuff
                await Task.Delay(500);

                await Console.Out.WriteLineAsync($"Date:{DateTime.Now.ToString("MM.dd.yyyy : HH:mm:ss.fff tt")}, Data={data}");

            }
            catch (Exception ex)
            {
                //Logging stuff
            }
        }
    }
}
