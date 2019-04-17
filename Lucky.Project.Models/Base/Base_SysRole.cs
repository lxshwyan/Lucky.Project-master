/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Models.Base
*文件名： Base_SysRole
*创建人： Lxsh
*创建时间：2019/3/26 11:00:42
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 11:00:42
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lucky.Project.Models.Base
{
     [Table("Base_SysRole")]
   public  class Base_SysRole
    {
        public String  Id { get; set; }
        public string RoleName { get; set; }
    }
}