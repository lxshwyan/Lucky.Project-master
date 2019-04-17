/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Core.CodeGenerator
*文件名： CodeGenerator
*创建人： Lxsh
*创建时间：2019/1/15 10:35:56
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/15 10:35:56
*修改人：Lxsh
*描述：
************************************************************************/
using Lucky.Proect.Core.DapperHelper;   
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;                              
using System.Reflection;
using Lucky.Proect.Core.Model;
using Lucky.Proect.Core.Extensions;
using Microsoft.Extensions.Options;

namespace Lucky.Proect.Core.CodeGenerator
{
    /// <summary> 
    /// 代码生成器。参考自：(yilezhu)参考自: Zxw.Framework.NetCore
    /// <remarks>
    /// 根据数据库表以及表对应的列生成对应的数据库实体
    /// </remarks>
    /// </summary>
    public class CodeGenerator
    {
        private CodeGenerateOption _options;
        private readonly string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符
        public CodeGenerator(IOptions<CodeGenerateOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value;;
            if (_options.ConnectionString.IsNullOrEmpty())
                throw new ArgumentNullException("不指定数据库连接串就生成代码，你想上天吗？");
            if (_options.DbType.IsNullOrEmpty())
                throw new ArgumentNullException("不指定数据库类型就生成代码，你想逆天吗？");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (_options.OutputPath.IsNullOrEmpty())
                _options.OutputPath = path;
            var flag = path.IndexOf("/bin");
            if (flag > 0)
                Delimiter = "/";//如果可以取到值，修改分割符
        }
        /// <summary>
        // 根据数据库表生成对应的模版代码
        /// </summary>
        /// <param name="isCoveredExsited">是否覆盖已存在的同名文件</param>
        public void GenerateTemplateCodesFromDatabase(bool isCoveredExsited = true)
        {    
            List<DbTable> tables = new List<DbTable>();
            using (var dbConnection = ConnectionFactory.CreateConnection(_options.DbType, _options.ConnectionString))
            {
                tables = dbConnection.GetCurrentDatabaseTableList(ConnectionFactory.GetDataBaseType(_options.DbType));
            }

            if (tables != null && tables.Any())
            {
                foreach (var table in tables)
                {
                    GenerateEntity(table, isCoveredExsited);


                    if (table.Columns.Any(c => c.IsPrimaryKey))
                    {
                        var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                        GenerateIRepository(table, pkTypeName, isCoveredExsited);
                        GenerateRepository(table, pkTypeName, isCoveredExsited);
                    }

                    GenerateIServices(table, isCoveredExsited); 
                    GenerateServices(table, isCoveredExsited);

                }
            }
        }

        /// <summary>
        /// 根据数据库表生成对应的实体代码
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isCoveredExisited"></param>
        private void GenerateEntity(DbTable table, bool isCoveredExsited = true)
        {
            var modelPath = _options.OutputPath + Delimiter + "Models"; ;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }
            var fullPath = modelPath + Delimiter + table.TableName + ".cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;

            var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var tmp = GenerateEntityProperty(table.TableName, column);
                sb.AppendLine(tmp);
            }
            var content = ReadTemplate("ModelTemplate.txt");
            
            content = content.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableDescription)
                .Replace("{ModelName}", table.TableName)
                .Replace("{ModelProperties}", sb.ToString())  
                .Replace("{DataBase}", _options.DataBase!=null?$"[{_options.DataBase}].[dbo].":"");
            SaveFile(fullPath, content);
        }
        /// <summary>
        /// 生成IService层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateServices(DbTable table, bool isCoveredExsited = true)
        {
            var iServicesPath = _options.OutputPath + Delimiter + "IServices";
            if (!Directory.Exists(iServicesPath))
            {
                Directory.CreateDirectory(iServicesPath);
            }
            var fullPath = iServicesPath + Delimiter + "I" + table.TableName + "Service.cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;
            var content = ReadTemplate("IServicesTemplate.txt");
            content = content.Replace("{Comment}", table.TableDescription)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IServicesNamespace}", _options.IServicesNamespace)
                .Replace("{ModelName}", table.TableName);
            SaveFile(fullPath, content);
        }
        /// <summary>
        ///  生成Services层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateIServices(DbTable table, bool isCoveredExsited = true)
        {
            var repositoryPath = _options.OutputPath + Delimiter + "Services";
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + table.TableName + "Service.cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;
            var content = ReadTemplate("ServiceTemplate.txt");
            content = content.Replace("{Comment}", table.TableDescription)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ServicesNamespace}", _options.ServicesNamespace)
                .Replace("{ModelName}", table.TableName);
            SaveFile(fullPath, content);

        }
        /// <summary>
        ///  生成IRepository层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateIRepository(DbTable table, string keyTypeName, bool isCoveredExsited = true)
        {
            var iRepositoryPath = _options.OutputPath + Delimiter + "IRepository";
            if (!Directory.Exists(iRepositoryPath))
            {
                Directory.CreateDirectory(iRepositoryPath);
            }
            var fullPath = iRepositoryPath + Delimiter + "I" + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;
            var content = ReadTemplate("IRepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableDescription)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IRepositoryNamespace}", _options.IRepositoryNamespace)
                .Replace("{ModelName}", table.TableName)
                .Replace("{KeyTypeName}", keyTypeName);
                SaveFile(fullPath, content);
        }
        /// <summary>
        /// 生成Repository层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateRepository(DbTable table, string keyTypeName, bool isCoveredExsited = true)
        {
            var repositoryPath = _options.OutputPath + Delimiter + "Repository";
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;
            var content = ReadTemplate("RepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableDescription)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{RepositoryNamespace}", _options.RepositoryNamespace)
                .Replace("{ModelName}", table.TableName)
                .Replace("{KeyTypeName}", keyTypeName);
            SaveFile(fullPath, content);
        }
        /// <summary>
        /// 生成属性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列</param>
        /// <returns></returns>
        private static string GenerateEntityProperty(string tableName, DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.ColumDescription))
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.ColumDescription);
                sb.AppendLine("\t\t/// </summary>");
            }
            if (column.IsPrimaryKey)
            {
                sb.AppendLine(" [Key]");
                //if (column.IsIdentity)
                //{
                //    sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                //}
                sb.AppendLine($"\t\tpublic {column.CSharpType} {column.ColumnName} " + "{get;set;}");
            }
            else
            {
                if (!column.IsNullable)
                {
                    sb.AppendLine("\t\t[Required]");
                }

                if (column.ColumnLength.HasValue && column.ColumnLength.Value > 0)
                {
                    sb.AppendLine($"\t\t[MaxLength({column.ColumnLength.Value})]");
                }
                //if (column.IsIdentity)
                //{
                //    sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                //}

                var colType = column.CSharpType;
                if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                    column.IsNullable)
                {
                    colType = colType + "?";
                }

                sb.AppendLine($"\t\tpublic {colType} {column.ColumnName} " + "{get;set;}");
            }

            return sb.ToString();
        }
        /// <summary>
        /// 从代码模板中读取内容
        /// </summary>
        /// <param name="templateName">模板名称，应包括文件扩展名称。比如：template.txt</param>
        /// <returns></returns>
        private string ReadTemplate(string templateName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var content = string.Empty;
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.CodeGenerator.CodeTemplate.{templateName}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 写入报错文件
        /// </summary>
        /// <param name="filePath">写入文件路径</param>
        /// <param name="content">内容</param>
        private async void SaveFileAsync(string filePath, string content)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var sw=new StreamWriter(fs))
            {
              await  sw.WriteAsync(content);
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 写入报错文件
        /// </summary>
        /// <param name="filePath">写入文件路径</param>
        /// <param name="content">内容</param>
        private  void SaveFile(string filePath, string content)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                 sw.Write(content);
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }
    }
}