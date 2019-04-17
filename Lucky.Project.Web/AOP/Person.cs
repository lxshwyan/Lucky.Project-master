using Lucky.Project.Framework.InterceptorAttribute;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.AOP
{
    public class Person
    {
            private ILogger<Person> _logger;
        public Person(ILogger<Person> logger)
        {
            _logger = logger;
        }
        [HystrixCommandAttribute(nameof(HelloFallBackAsync))]
        public virtual async Task<string> HelloAsync(string name)//需要是虚方法
        {
         
               _logger.LogError("hello" + name);
            String s = null;
            s.ToString();
            return "ok";
        }
        public async Task<string> HelloFallBackAsync(string name)
        {
            _logger.LogError("执行失败" + name);
          
            return "fail";
        }


        [HystrixCommand(nameof(AddFall))]
        public virtual int Add(int i, int j)
        {
            String s = null;
            //  s.ToArray();
            return i + j;
        }
        public int AddFall(int i, int j)
        {
               _logger.LogError("执行失败" + i);
               return 0;
        }
    }
}
