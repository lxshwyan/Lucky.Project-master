/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Core.Model
*文件名： CodeGenerateOption
*创建人： Lxsh
*创建时间：2019/1/15 13:22:16
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/15 13:22:16
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Proect.Core.Model
{
    public  class CodeGenerateOption
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 代码生成时间
        /// </summary>
        public string GeneratorTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public string ModelsNamespace { get; set; }
        /// <summary>
        /// 仓储接口命名空间
        /// </summary>
        public string IRepositoryNamespace { get; set; }
        /// <summary>
        /// 仓储命名空间
        /// </summary>
        public string RepositoryNamespace { get; set; }
        /// <summary>
        /// 服务接口命名空间
        /// </summary>
        public string IServicesNamespace { get; set; }
        /// <summary>
        /// 服务命名空间
        /// </summary>
        public string ServicesNamespace { get; set; }
         /// <summary>
         /// 数据库连接字符串
         /// </summary>
        public string ConnectionString { get; set; }
       /// <summary>
       /// 数据库类型
       /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBase { get; set; }
    }
}