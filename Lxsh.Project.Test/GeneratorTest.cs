using Lucky.Proect.Core.CodeGenerator;
using Lucky.Proect.Core.DapperHelper;
using Lucky.Proect.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Lxsh.Project.Test
{
    public class GeneratorTest
    {
        [Fact]
        public void GeneratorModelForSqlServer()
        {

            var serviceProvider = Common.BuildServiceForSqlServer();
            var codeGenerator = serviceProvider.GetRequiredService<CodeGenerator>(); 
             codeGenerator.GenerateTemplateCodesFromDatabase(true);
            Assert.Equal("SQLServer", DatabaseType.SqlServer.ToString(), ignoreCase: true);
        }
    }
}
