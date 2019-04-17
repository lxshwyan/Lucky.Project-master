/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Core.Model
*文件名： DbTableColumn
*创建人： Lxsh
*创建时间：2019/1/15 10:50:25
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/15 10:50:25
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
    /// <summary>
    /// 数据库中列属性
    /// </summary>
    public class DbTableColumn
    {
        /// <summary>
        /// 列名
        /// /// </summary>
        public string ColumnName { get; set; }
         /// <summary>
         /// 是否自增
         /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool  IsPrimaryKey { get; set; }
        /// <summary>
        ///  字段数据类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 字段数据长度
        /// </summary>
        public long? ColumnLength { get; set; }
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string ColumDescription { get; set; }
        /// <summary>
        /// C#数据类型
        /// </summary>
        public string CSharpType { get; set; }


    }
}