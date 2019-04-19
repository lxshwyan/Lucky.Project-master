
using Lucky.Proect.Core;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IScheduler = Quartz.IScheduler;

namespace Lucky.Project.Web.Quarzs
{
    public class QuartzStartup
    {
       
         private readonly ILogger<QuartzStartup> _logger;
         private readonly ISchedulerFactory _schedulerFactory;
          private IScheduler _scheduler;
         private readonly IJobFactory _iocJobfactory;
        public QuartzStartup(IJobFactory iocJobfactory, ILogger<QuartzStartup> logger,ISchedulerFactory schedulerFactory)
        {
           this._logger = logger;
           this._schedulerFactory = schedulerFactory;
           this._iocJobfactory =iocJobfactory;


        }
        public async Task<string> Start()
        {
            //1、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
             //_scheduler.JobFactory = _iocJobfactory;
            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
                            .Build();
            //4、创建任务
            var jobDetail = JobBuilder.Create<UserInfoSyncjob>()
                            .WithIdentity("job", "group")
                            .Build();
            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult("将触发器和任务器绑定到调度器中完成");
        }
        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                _scheduler = null;
            else
            {
            }
            _logger.LogCritical("Schedule job upload as application stopped");
        }
    }
}
