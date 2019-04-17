/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Models
*文件名： Base_User
*创建人： Lxsh
*创建时间：2019/3/20 15:59:39
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/20 15:59:39
*修改人：Lxsh
*描述：

************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lucky.Project.Models
{
    [Table("Base_User")]  
    public partial class Base_User
    {    

        public String Id { get; set; }

      
        public string Account { get; set; }

     
        public string UserName { get; set; }
        public string RealName { get; set; }


        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

    
        public string Sex { get; set; }

        public bool Enabled { get; set; }  

        public DateTime CreationTime { get; set; }

        public int LoginFailedNum { get; set; }

        public DateTime? AllowLoginTime { get; set; }

        public bool LoginLock { get; set; }

        public DateTime? LastLoginTime { get; set; }

     
        public string LastIpAddress { get; set; }

        public DateTime? LastActivityTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }

        public String ModifierID { get; set; }

        public String CreatorID{ get; set; }
     
         public string Remark { get; set; }
         public string DepartmentID { get; set; }
         public DateTime? Birthday { get; set; }

         public bool IsAdmin { get; set; }
        

      
    }
}