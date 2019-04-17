using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using System.Linq;
using Lucky.Project.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Lucky.Project.IServices;
using Lucky.Project.Web.Security;
using Newtonsoft.Json.Linq;
using Lucky.Project.ViewModels;       

namespace Lucky.Project.Web.Security
{
    public class AdminAuthService : IAdminAuthService
    {
          private IBaseUserService _sysUserService;
         private IHttpContextAccessor _httpContextAccessor;
          private IBaseMenusRoleService _BaseMenusRoleService;
         private IBaseUserRoleService _BaseUserRoleService;
        private IBaseMenusService _BaseMenusService;
          private IBaseRolePermissionService _BaseRolePermissionService ;
     
        public AdminAuthService(IHttpContextAccessor httpContextAccessor, IBaseUserService  sysUserService ,
            IBaseMenusRoleService BaseMenusRoleService,IBaseUserRoleService BaseUserRoleService,
             IBaseMenusService BaseMenusService, IBaseRolePermissionService BaseRolePermissionService
            ) 
        {
        
           this._httpContextAccessor = httpContextAccessor;
            this._sysUserService = sysUserService;
            _BaseMenusRoleService = BaseMenusRoleService;
            _BaseUserRoleService = BaseUserRoleService;
            _BaseMenusService = BaseMenusService;
            _BaseRolePermissionService = BaseRolePermissionService;
            
        }
      
        public bool authorize(ActionExecutingContext context)
        {
            var user = getCurrentUser();
            if (user == null)
                return false;
            //如果是超级管理员
            //if (user.IsAdmin) return true;
            string action = context.ActionDescriptor.RouteValues["action"];
            string controller = context.ActionDescriptor.RouteValues["controller"];

            return authorize(action, controller);
        }
        /// <summary>
        /// 私有方法，判断权限
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        private bool authorize(string action, string controller)
        {
            var user = getCurrentUser();
            if (user == null)
                return false;
            //如果是超级管理员
            if (user.IsAdmin) return true;
            var list = getMyCategories(user);
            if (list == null) return false;
            return list.Any(o => o.Controller != null && o.Action != null ||
            o.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase)
            && o.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase));
        }
        public bool authorize(string routeName)
        {
            var user = getCurrentUser();
            if (user == null)
                return false;
            //如果是超级管理员
            if (user.IsAdmin) return true;
            var list = getMyCategories(user);
            if (list == null) return false;
            return list.Any(o => o.RouteName != null &&
            o.RouteName.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// 获取我的权限数据
        /// </summary>
        /// <returns></returns>
        public List<Base_Menu> getMyCategories()
        {
            var user = getCurrentUser();
            return getMyCategories(user);
        }

        /// <summary>
        /// 私有方法，获取当前用户的方法数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<Base_Menu> getMyCategories(Base_User user)
        {
            var list = _BaseMenusService.getAll();
            if (user == null) return null;
            if (user.IsAdmin) return list;

            //获取权限数据
            var userRoles = _BaseUserRoleService.getAll();
            if (userRoles == null || !userRoles.Any()) return null;
            var roleIds = userRoles.Where(o => o.UserId == user.Id).Select(x => x.RoleId).Distinct().ToList();
            var permissionList = _BaseRolePermissionService.getAll();
            if (permissionList == null || !permissionList.Any()) return null;
            var categoryIds = permissionList.Where(o => roleIds.Contains(o.RoleId)).Select(x => x.MenuId).Distinct().ToList();
            if (!categoryIds.Any())
                return null;
            list = list.Where(o => categoryIds.Contains(o.Id)).ToList();
            return list;
        }
        public Base_User getCurrentUser()
        {    
            var result = _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAdminAuthInfo.AuthenticationScheme).Result;
            if (result.Principal == null)
                return null;
            var token = result.Principal.FindFirstValue(ClaimTypes.Sid);
          
            return _sysUserService.GetUserInfoBytoken(token ?? "");
        }

        /// <summary>
        /// 保存等状态
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        public void signIn(string token, string name)
        {
            var claims = new List<Claim>
                {      
                    new Claim("LoginCount","1"),
                    new Claim("LoginLastIp","127.0.0.1") ,
                    new Claim("LoginLastTime",DateTime.Now.ToString()),
                };
            var claimsIdentity = new ClaimsIdentity(
                     claims, CookieAdminAuthInfo.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, token));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, name)); 
            ClaimsPrincipal claimsPrincipal=new ClaimsPrincipal(claimsIdentity); 
            _httpContextAccessor.HttpContext.SignInAsync (CookieAdminAuthInfo.AuthenticationScheme, claimsPrincipal);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public void signOut()
        {
          _httpContextAccessor.HttpContext.SignOutAsync(CookieAdminAuthInfo.AuthenticationScheme);
        }
    }
}
