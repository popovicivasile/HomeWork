using Quartz.Spi;
using Quartz;

namespace HomeWork.Core.TimeJobs
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceProvider serviceScopeFactory;


        public JobFactory(IServiceProvider serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }

        }
        public void ReturnJob(IJob job)
        {
          
        }
    }
}
