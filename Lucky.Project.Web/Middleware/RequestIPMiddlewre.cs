using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Middleware
{
    public class RequestIPMiddlewre
    {
        private readonly RequestDelegate _next; //定义请求委托
        private readonly ILogger _logger; //定义日志
        public RequestIPMiddlewre(RequestDelegate next, ILoggerFactory log)
        {
            _next = next;
            _logger = log.CreateLogger<RequestIPMiddlewre>();
        }
        public async Task Invoke(HttpContext context)
        {
            string ip = context.Connection.RemoteIpAddress.ToString(); 
       
            if (context.Request.Path.ToString().Contains("lxsh"))
            {
                context.Response.ContentType = "text/plain;charset=UTF-8";
                await context.Response.WriteAsync($"Hello World!当前登录时间：{DateTime.Now.ToString()}User IP：{ip}");
            }
             await _next.Invoke(context);   
        }

    }
}
