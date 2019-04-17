
using Lucky.Proect.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Filter
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
       public static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext filterContext)
        {
            logger.Error(filterContext.Exception);
            var result = new BaseResult()
            {
                ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode,//系统异常代码
                ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg,//系统异常信息
            };
            filterContext.Result = new ObjectResult(result);
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.ExceptionHandled = true;
        }
    }
}
