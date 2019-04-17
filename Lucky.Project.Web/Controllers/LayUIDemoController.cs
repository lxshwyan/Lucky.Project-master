using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucky.Proect.Core.Extensions;
using Lucky.Project.IServices;
using Lucky.Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lucky.Project.Web.Controllers
{
    
    public class LayUIDemoController : Controller
    {
        private readonly IBaseOperateLogService _BaseOperateLogService;
        public LayUIDemoController(IBaseOperateLogService BaseOperateLogService)
        {
            _BaseOperateLogService = BaseOperateLogService;
        }
        public IActionResult Index()
        {
            return View();
        }
         [HttpGet]
        public string GetLogInfo(Base_OperateLogSearchArg searchArg)
        {
           return _BaseOperateLogService.SearchLog(searchArg).ObjectToJSON();
        }
    }
}