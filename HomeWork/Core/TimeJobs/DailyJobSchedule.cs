using Quartz.Impl;
using Quartz;

namespace HomeWork.Core.TimeJobs
{
    public class DailyJobSchedule
    {

        public static async Task Start(IConfiguration config, IServiceProvider serviceProvider)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler scheduler = await sf.GetScheduler();
            scheduler.JobFactory = new JobFactory(serviceProvider);
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<SendAppointmentReminder>()
                .WithIdentity("dailyWorkJob", "group1")
                .Build();

            string startTime = config.GetValue<string>("ServiceData:MidDayLoaderPeriod");

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("dailyWorkJob", "group1")
                .StartNow()
                .WithCronSchedule(startTime)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
