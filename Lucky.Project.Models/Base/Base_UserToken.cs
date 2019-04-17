/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Models.Base
*文件名： Base_UserToken
*创建人： Lxsh
*创建时间：2019/3/21 12:26:18
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/21 12:26:18
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lucky.Project.Models
{
  [Table("Base_UserToken")]
   public class Base_UserToken
    {      
        public String Id { get; set; }

        public String UserId { get; set; }

        public DateTime ExpireTime { get; set; }
    }
}