/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Models
*文件名： Base_OperateLog
*创建人： Lxsh
*创建时间：2019/3/21 12:49:13
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/21 12:49:13
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lucky.Project.Models
{
    [Table("Base_OperateLog")]
    public class Base_OperateLog
    {
        public String Id { get; set; }
        public String UserId { get; set; }
        public string OperateType { get; set; }
        public string OperateCotent { get; set; }
        public DateTime? CreateTime { get; set; }
        public string  IpAddress { get; set; }

    }
}