using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lucky.Project.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);  
        }    
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {    
          
            if (env.IsDevelopment())               
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            string ip =Program.ip;         
            int port = Program.port;

            //向consul注册服务  
            var client = new ConsulClient(obj =>
            {
                obj.Address = new Uri(Configuration["ConsulUrl"]);
                obj.Datacenter = "dc1";
            });
            var result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "Lucky.Project.API.Service" + Guid.NewGuid(),//服务编号，不能重复，用Guid最简单
                Name = "Lucky.Project.API.Service",//服务的名字
                Address = ip,//我的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
                Port = port,//我的端口
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://{ip}:{port}/api/health",//健康检查地址,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });
        }
    }
}
