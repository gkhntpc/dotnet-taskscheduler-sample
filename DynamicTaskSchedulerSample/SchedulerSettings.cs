using System;
using System.Collections.Generic;

namespace DynamicTaskSchedulerSample
{
    internal class SchedulerSettings
    {
        public static string SectionName => "SchedulerSettings";
        public string Name { get; set; }
        public string Method { get; set; }
        public int Interval { get; set; }
        public string GroupName { get; set; }
        public string Data { get; set; }
    }
}
