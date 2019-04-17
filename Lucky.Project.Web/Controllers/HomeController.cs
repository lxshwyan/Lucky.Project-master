using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lucky.Proect.Core.Cache;
using Microsoft.AspNetCore.Mvc;
using Lucky.Project.Web.Models;
using Lucky.Project.Web.Filter;
using Lucky.Project.Web.Security;
using Lucky.Proect.Core.Extensions;
using Lucky.Project.ViewModels;
using Lucky.Project.Models;
using Lucky.Project.IServices;

namespace Lucky.Project.Web.Controllers
{
   
    public class HomeController : BaseController
    {
        private readonly   IAdminAuthService _adminAuthService;
        private readonly IBaseUserRoleService _BaseUserRoleService;
        private readonly IBaseMenusRoleService _BaseMenusRoleService;
         private readonly IBaseMenusService _BaseMenusService;
        public HomeController(ICache cache,IAdminAuthService adminAuthService
            ,IBaseUserRoleService BaseUserRoleService,IBaseMenusRoleService BaseMenusRoleService,
            IBaseMenusService BaseMenusService)
        {
            _adminAuthService = adminAuthService;
            _BaseUserRoleService = BaseUserRoleService;
            _BaseMenusRoleService = BaseMenusRoleService;
            _BaseMenusService = BaseMenusService;  
          
        }  
        public IActionResult Index()
        {
            var user = _adminAuthService.getCurrentUser();
            return View(user);
        }
        /// <summary>
        /// 控制中心
        /// </summary>
        /// <returns></returns>
        public IActionResult Main()
        {
            ViewData["LoginCount"] = User.Claims.FirstOrDefault(x => x.Type == "LoginCount")?.Value;
            ViewData["LoginLastIp"] = User.Claims.FirstOrDefault(x => x.Type == "LoginLastIp")?.Value;
            ViewData["LoginLastTime"] = User.Claims.FirstOrDefault(x => x.Type == "LoginLastTime")?.Value;
         
            return View();
        }
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            _adminAuthService.signOut();
            return RedirectToRoute("BaseLogin");
        }
        public string GetMenu()
        {
            var menus= getMyCategories().GenerateTree(x => x.Id, x => x.ParentId,"0");

            return menus.ObjectToJSON();
        }

        public List<MenuNavView> getMyCategories()
        {
            Base_User user = _adminAuthService.getCurrentUser();
            if (user.IsAdmin)
            {
                return _BaseMenusService.getAllMenuNavView();
            }
            Base_UserRoleMap userRoleMap = _BaseUserRoleService.GetRolesByUserId(user?.Id).FirstOrDefault();
        
            return _BaseMenusRoleService.GetMenusByRoleId(userRoleMap?.RoleId);
           
        }
    }
}
