using Lucky.Proect.Core;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Quarzs
{
    public class IOCJobFactory : IJobFactory
    {
     
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type type = bundle.JobDetail.JobType;  
            return  EnginContext.Current.Resolve(type) as IJob;

        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
