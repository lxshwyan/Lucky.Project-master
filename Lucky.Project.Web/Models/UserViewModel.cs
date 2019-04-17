using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Models
{
    public class UserInfoViewModel
    {
         public string UserName { get; set; }        
         public string Content { get; set; }
    }



    public class GroupViewModel
    {
        public string Name { get; set; }
    }
}
