
using Lucky.Project.Models;
using Lucky.Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.IServices
{
    public interface IBaseUserRoleService
    {
           List<Base_UserRoleMap> GetRolesByUserId(string UserId);
           List<Base_UserRoleMap> getAll();
     
    }
      
}
