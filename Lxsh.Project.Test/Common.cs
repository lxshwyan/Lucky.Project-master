/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lxsh.Project.Test
*文件名： Common
*创建人： Lxsh
*创建时间：2019/3/27 11:44:18
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/27 11:44:18
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core;
using Lucky.Proect.Core.CodeGenerator;
using Lucky.Proect.Core.DapperHelper;
using Lucky.Proect.Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lxsh.Project.Test
{
   public   class Common
    {
        /// <summary>
        /// 构造依赖注入容器，然后传入参数
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider BuildServiceForSqlServer()
        {
            var services = new ServiceCollection();
         

           services.Configure<CodeGenerateOption>(options =>
           {
                options.ConnectionString = "Data Source=.;Initial Catalog=LuckyProjectDB;User ID=sa;Password=123456;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                options.DbType = DatabaseType.SqlServer.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType
                options.Author = "lxsh";//作者名称
                options.OutputPath = "C:\\LuckyProjectCodeGenerator";//模板代码生成的路径
                options.ModelsNamespace = "Lucky.Project.Models.Models";//实体命名空间
                options.IRepositoryNamespace = "Lucky.Project.IRepository";//仓储接口命名空间
                options.RepositoryNamespace = "Lucky.Project.Repository.SqlServer";//仓储命名空间
                options.IServicesNamespace = "Lucky.Project.IServices";//服务接口命名空间
                options.ServicesNamespace = "Lucky.Project.Services";//服务命名空间


            }); 
            services.Configure<ConfigOption>("LuckyConfig", GetConfiguration().GetSection("ConfigOption"));
            services.AddScoped<CodeGenerator>();
          
            return services.BuildServiceProvider(); //构建服务提供程序
        }
        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}