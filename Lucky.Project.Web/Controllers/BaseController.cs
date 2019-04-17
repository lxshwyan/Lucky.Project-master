using Lucky.Project.Framework;
using Lucky.Project.Web.Filter;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Lucky.Project.Web.Hub;

namespace Lucky.Project.Web.Controllers
{
    [AdminAuthFilter]
    public class BaseController : Controller
    {
        // public static Logger logger = LogManager.GetCurrentClassLogger();
        public Logger _logger;
        private AjaxResult<string> _ajaxResult;
        public BaseController()
        {

            this._ajaxResult = new AjaxResult<string>()
            {
                Data = "初始化信息",
                Message = "初始化信息",
                Success = true,
                StatusCode = 200
            };
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// ajax请求的数据结果
        /// </summary>
        public AjaxResult<string> AjaxData
        {
            get
            {
                return _ajaxResult;
            }
        }
        /// <summary>
        /// 日志写入
        /// </summary>
        public Logger Logger
        {
            get
            {
                return _logger;
            }
        }
    }
}