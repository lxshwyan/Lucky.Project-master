/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Core.Model
*文件名： DbTable
*创建人： Lxsh
*创建时间：2019/1/15 10:47:22
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/15 10:47:22
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
   public class DbTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表描述
        /// </summary>
        public string TableDescription { get; set; }
        /// <summary>
        /// 字段集合
        /// </summary>
        public virtual List<DbTableColumn> Columns { get; set; } = new List<DbTableColumn>();
    }
}