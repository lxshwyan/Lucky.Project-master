using Lucky.Proect.Core;
using Lucky.Project.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Lucky.Project.Web.Filter
{
    /// <summary>
    /// 登录状态过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AdminAuthFilterAttribute : Attribute, IResourceFilter
    { 
        public void OnResourceExecuted(ResourceExecutedContext context)
        {  

        }     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {  
            if (!context.ActionDescriptor.FilterDescriptors.Any(o => o.Filter.GetType().Name.Equals("AllowAnonymousFilter")))
            {  
                var _adminAuthService = EnginContext.Current.Resolve<IAdminAuthService>();
                var user = _adminAuthService.getCurrentUser();
                if (user == null || !user.Enabled)
                 context.Result = new RedirectToRouteResult("BaseLogin", new { returnUrl = context.HttpContext.Request.Path });
            }
        }
    }
}
