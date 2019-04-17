/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Models.Base
*文件名： Base_UserRoleMap
*创建人： Lxsh
*创建时间：2019/3/26 11:02:00
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 11:02:00
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lucky.Project.Models
{
      [Table("Base_UserRoleMap")]
   public class Base_UserRoleMap
    {
        public string  Id { get; set; }
        public string  UserId { get; set; }
        public string RoleId { get; set; }
    }
}