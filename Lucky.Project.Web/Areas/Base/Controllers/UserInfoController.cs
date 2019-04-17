using System;
using System.Collections.Generic;               
using System.Linq;
using System.Threading.Tasks;
using Lucky.Project.IServices;
using Lucky.Project.Models;
using Lucky.Project.ViewModels;
using Lucky.Project.Web.Security;
using Lucky.Project.Web.Validation;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace Lucky.Project.Web.Areas.Base.Controllers
{
    public class UserInfoController : BaseAreaController
    {
  
        private IBaseUserService _baseUserService;
        private IAdminAuthService _authenticationService;
        public UserInfoController(IBaseUserService baseUserService, IAdminAuthService authenticationService)
        {
            _baseUserService = baseUserService;
            this._authenticationService = authenticationService;
        }
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            Base_User base_User = _authenticationService.getCurrentUser();

            ChangePasswordModel model = new ChangePasswordModel();
            model.Accont = base_User?.Account;
            return View(model);
        }
        [Route("UserInfo")]
        public IActionResult UserInfo()
        {
            var user = _authenticationService.getCurrentUser();
            return View(user);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            #region 属性判断
            ChangePasswordModelValidation validation = new ChangePasswordModelValidation();
            ValidationResult results = validation.Validate(model);
            if (!results.IsValid)
            {
                AjaxData.Success = false;
                AjaxData.Message = results.ToString("||");
                return Json(AjaxData);
            }
            #endregion

            _baseUserService.ChangePassword(model);
            return View();
        }
    }
}