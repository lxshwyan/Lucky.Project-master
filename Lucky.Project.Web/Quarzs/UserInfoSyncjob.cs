
using Lucky.Proect.Core;
using Lucky.Proect.Core.Cache;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Quarzs
{
    public class UserInfoSyncjob : IJob
    {
        private readonly ILogger<UserInfoSyncjob> _logger;
        private readonly ICache _cache;
        public UserInfoSyncjob()
        {
            _cache = EnginContext.Current.Resolve<ICache>();
            _logger = EnginContext.Current.Resolve<ILogger<UserInfoSyncjob>>();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
                        {
                           
                             Console.WriteLine("开始执行");
                            _logger.LogInformation("开始执行Job");
                            using (StreamWriter sw = new StreamWriter(@"C:\sdklog\1.txt", true, Encoding.UTF8))
                            {
                                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                            }
                        });
        }
    }
}
