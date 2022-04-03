using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Quartz
{
    public class QuartzManager
    {
        public static Action<Type> JoinToQuartz = (type) =>
        {
            StdSchedulerFactory.GetDefaultScheduler().Result.Start();
            var obj = Activator.CreateInstance(type);
            string cron = type.GetProperty("Cron").GetValue(obj).ToString();
            var jobDetail = JobBuilder.Create(type)
                                      .WithIdentity(type.Name)
                                      .Build();

            var jobTrigger = TriggerBuilder.Create()
                                           .WithIdentity(type.Name + "Trigger")
                                           .StartNow()
                                           .WithCronSchedule(cron)
                                           .Build();

            StdSchedulerFactory.GetDefaultScheduler().Result.ScheduleJob(jobDetail, jobTrigger);
     
        };
    }
}
