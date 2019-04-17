using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lucky.Proect.Core;
using Lucky.Project.Framework.Extensions ;
using Lucky.Proect.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using NLog.Web;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Lucky.Proect.Core.Cache;
using Microsoft.Extensions.Caching.Memory;
using Lucky.Project.Web.Filter;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Lucky.Project.Web.Security;
using AutoMapper;
using Lucky.Project.Web.Hub;
using Lucky.Project.Framework.Register;
using Microsoft.AspNetCore.SignalR;
using Lucky.Proect.Core.RabbitMQ;
using Lucky.Proect.Core.Extensions;        
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Lucky.Project.Web.Models;
using AspectCore.Extensions.DependencyInjection;
using Lucky.Project.Web.Middleware;

namespace Lucky.Project.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           services.Configure<ConfigOption>("LuckyConfig", Configuration.GetSection("ConfigOption"));
            //解决视图输出内容中文编码问题
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.Configure<CookiePolicyOptions>(options =>
            {      
                options.CheckConsentNeeded = context => false;  //当为true时不能保存cookie到客户端
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc(option =>
                option.Filters.Add(new GlobalExceptionFilter()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices()
                .AddFluentValidation(fv =>
                {
                 
                    //去掉其他的验证，只使用FluentValidation的验证规则
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

             services.AddAuthentication(o =>
             {
                 o.DefaultAuthenticateScheme = CookieAdminAuthInfo.AuthenticationScheme;
                 o.DefaultChallengeScheme = CookieAdminAuthInfo.AuthenticationScheme;
             })
            .AddCookie(CookieAdminAuthInfo.AuthenticationScheme,options =>
           {
             
               options.LoginPath = "/login";
               options.LogoutPath = "/Account/Logout";
               options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
           });
             //services.AddAntiforgery(options =>
             //{
             //       // Set Cookie properties using CookieBuilder properties†.
             //    options.FormFieldName = "AntiforgeryKey_yilezhu";
             //    options.HeaderName = "X-CSRF-TOKEN-yilezhu";
             //    options.SuppressXFrameOptionsHeader = false;
             //});
            string str = Configuration["ConfigOption:ConnectionString"];
            services.AddDbContextPool<GeneralDbContext>(options => options.UseSqlServer(Configuration["ConfigOption:ConnectionString"]));
            services.AddSession(options =>
             {
                 options.IdleTimeout = TimeSpan.FromMinutes(15);
                 options.Cookie.HttpOnly = true;    //设置为后台只读模式,前端无法通过JS来获取cookie值,可以有效的防止XXS攻击
             });
            
            #region 缓存 读取配置是否使用哪种缓存模式     
            services.AddMemoryCache();
            if (Convert.ToBoolean(Configuration["ConfigOption:IsUseRedis"]))
            {
                services.AddDistributedRedisCache(option =>
                   {
                       option.Configuration = Configuration["ConfigOption:Redisconfig"];
                       option.InstanceName = "RedisCacheTest";
                   });
                services.AddSingleton<ICache, RedisCacheService>();
            }
            else
            {      
                //services.AddSingleton<ICache, MemoryCacheService>();
                  services.AddSingleton<ICache, NuLLCacheService>();
            }
            #endregion
            //DI了AutoMapper中需要用到的服务，其中包括AutoMapper的配置类 Profile
            services.AddAutoMapper();
           // services.AddDirectoryBrowser();

         ///   services.AddSingleton<ChatHub>() //可以通过全局单例注入的形式全局调用里面方法
            services.AddSignalR();

            //程序集依赖注入
            services.AddAssembly("Lucky.Project.Services");
            services.AddAspectCoreServices("Lucky.Project.Web");
            //泛型注入到DI里面
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IAdminAuthService, AdminAuthService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRegisterApplicationService, RegisterApplicationService>();
            //services.AddScoped<IhubContextRegister, hubContextRegister>();
            EnginContext.Initialize(new GeneralEngine(services.BuildServiceProvider()));

             return services.BuildAspectInjectorProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IHubContext<ChatHub> chatHub,IApplicationLifetime appLifetime )
        {
        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            #region AddNLog
            //添加NLog  
            loggerFactory.AddNLog();
            //读取Nlog配置文件 
            env.ConfigureNLog("nlog.config");
            #endregion
            app.UseRequestIP(); 

            app.UseAuthentication();                                                          
            app.UseStaticFiles();
            #region  自定义静态文件路径
            #region 测试代码
            //  string webRootPath = env.WebRootPath;
            // string contentRootPath = env.ContentRootPath;
            //  启用静态文件
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"MyStaticFiles")),
            //    RequestPath = new PathString("/StaticFiles"),

            //});

            // 这段代码允许在访问 http://<app>/MyImages 时可浏览 MyStaticFiles文件夹的目录，其中包括该文件夹下的每一个文件与文件夹：
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //   Path.Combine(env.ContentRootPath, @"MyStaticFiles")),
            //    RequestPath = new PathString("/MyImages")
            //});


            // 你可能希望启用静态文件、默认文件以及浏览 MyStaticFiles 目录
            //app.UseFileServer(new FileServerOptions()                                      
            //{                                                                              
            //    FileProvider = new PhysicalFileProvider(                                   
            //    Path.Combine(env.ContentRootPath, @"MyStaticFiles")), 
            //    RequestPath = new PathString("/StaticFiles"),                              
            //    EnableDirectoryBrowsing = true                                             
            //});
            #endregion

            #region 实战简单文件服务器
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            // add new mapping.
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            // replace an existing mepping.
            provider.Mappings[".rtf"] = "application/x-msdownload";
            // remove mp4 vidios.
          //  provider.Mappings.Remove(".mp4");
            provider.Mappings[".log"] = "text/plain";
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(@"E:\"),
                ServeUnknownFileTypes = true,
                RequestPath = new PathString("/StaticFiles"),
                ContentTypeProvider = provider,
                DefaultContentType = "application/x-msdownload", // 设置未识别的MIME类型一个默认z值

            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
               FileProvider = new PhysicalFileProvider(@"E:\"),
               RequestPath = new PathString("/StaticFiles"),
            });
            #endregion

            #endregion
            app.UseCookiePolicy();
            app.UseSession();
            app.UseSignalR(route =>
           {
               route.MapHub<ChatHub>("/Chathub");
           });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }); 
            app.UseMvc(routes =>
           {
               routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Login}/{action=LoginIndex}/{id?}"
                );
           });

            #region 它能用来处理网站启动后，以及停止网站的任务。
              ILogger _logger = loggerFactory.CreateLogger<Startup>();
            appLifetime.ApplicationStarted.Register(() =>
            {
               _logger.LogInformation("Moonglade started.");
            });
            appLifetime.ApplicationStopping.Register(() =>
              {
                  _logger.LogInformation("Moonglade is stopping...");
              });
            appLifetime.ApplicationStopped.Register(() =>
            {
                _logger.LogInformation("Moonglade stopped.");
            });
            #endregion
            EnginContext.Current.Resolve<IRegisterApplicationService>().initRegister();
          //EnginContext.Current.Resolve<IhubContextRegister>().InitMessageRegister();// 通过次方法发送消息客户端无法接收？？？？？？？？？
           // InitMessageRegister(chatHub); 
        }
        /// <summary>
        /// 初始化消息
        /// </summary>
        /// <param name="chatHub"></param>
        public void InitMessageRegister(IHubContext<ChatHub> chatHub)
        {
            string strRabbitMQconfig = Configuration["ConfigOption:RabbitMQconfig"];
            MQHelper tMQHelper = MQHelperFactory.CreateBus(strRabbitMQconfig);

            #region 模拟第三方消息发送
            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        chatHub.Clients.All.SendAsync("Notify", $" {DateTime.Now}:->{new Random().Next(1, 10000)}");
            //        Task.Delay(5000);
            //    }
            //});
            int i = 0;
            string strErrorMsg = "";
            Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    i++;
                    var bodyMsg = new SystemMessage()
                    {
                        Content = $"定时第{i}条信息",
                        DateTime = DateTime.Now.ToString(),
                        Title = "四方博瑞安防平台信息",
                        Type = "log".ObjectToJSON()
                    };
                    var message = new RabbitMQMsg { Body = bodyMsg.ObjectToJSON(), Category = CategoryMessage.System, Dst = "预留字段", SendTime = DateTime.Now.ToString(), Src = "消息来源" };

                    tMQHelper.TopicPublish(message.Category.ToString() + ".xA", message, ref strErrorMsg);
                    Task.Delay(5000);

                };
            });
            #endregion

            #region 通过signar发送到客户端
            tMQHelper.TopicSubscribe(Guid.NewGuid().ToString(), s =>
          {
              Console.WriteLine("当前收到信息：" + s.Body.FromJson<SystemMessage>().Content);


              foreach (var connection in ChatHub.UserList)
              {

                  if (chatHub != null && connection.Value != null)
                  {
                      foreach (var item in connection.Value)
                      {
                          chatHub.Clients.Client(item).SendAsync("Recv",  new ChatMessage() { Type=0, UserName="RabbitQM",Content=s.Body.FromJson<SystemMessage>().Content }
                             );
                      }
                   
                  }
              }
                  //   chatHub.Clients.All.SendAsync("Recv", $" {DateTime.Now}:->{new Random().Next(1, 10000)}");
               }, true, CategoryMessage.System.ToString() + ".*", CategoryMessage.Alarm.ToString() + ".*");
            #endregion   
        }
    }
}
