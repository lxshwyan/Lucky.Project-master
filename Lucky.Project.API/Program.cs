using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lucky.Proect.Core.Helper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lucky.Project.API
{
    public class Program
    {
         public static int port;
          public static string ip;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();
            String ip = config["ip"];
            int port = Convert.ToInt32(config["port"]);
            if (port == 0)
            {
                //随机产生可用端口号
                port = PortHelper.GetRandAvailablePort();

            }
            Program.port = port;
            if (ip==null)
            {
                ip = "127.0.0.1";
            }
            Program.ip = ip;
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://{ip}:{port}");
        }
    }
}
