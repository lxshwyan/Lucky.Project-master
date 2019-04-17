using System;
using System.Collections.Generic;         
using System.Linq;
using System.Threading.Tasks;
using Lucky.Proect.Core.Helper;
using Lucky.Project.IServices;
using Lucky.Project.ViewModels;
using Lucky.Project.Web.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Lucky.Proect.Core.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Lucky.Project.Web.Security;
using Lucky.Project.Models;

namespace Lucky.Project.Web.Areas.Base.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseAreaController
    {
        private const string R_KEY = "R_KEY";
        private const string CaptchaCodeSessionName = "CaptchaCode";
        private IBaseUserService _baseUserService;
        private IAdminAuthService _authenticationService;    
        public LoginController(IBaseUserService baseUserService,IAdminAuthService authenticationService)
        {
              _baseUserService = baseUserService;
             this._authenticationService = authenticationService;
        }      
        [Route("login", Name = "BaseLogin")]
        public IActionResult LoginIndex()
        {     
            string r = EncryptorHelper.GetMD5(Guid.NewGuid().ToString());
            HttpContext.Session.SetString(R_KEY, r);
            LoginModel loginModel = new LoginModel() { R = r };
            return View(loginModel);
        }
        [HttpPost] 
        [Route("login")]
        public IActionResult LoginIndex(LoginModel model)
        {
            #region 判断验证码 
            //if (!ValidateCaptchaCode(model.CaptchaCode))
            //{
            //    AjaxData.Success = false;
            //    AjaxData.Message = ResultCodeAddMsgKeys.SignInCaptchaCodeErrorMsg;
            //    return Json(AjaxData);
            //}
            #endregion

            #region 属性判断
            LoginModelValidation validation = new LoginModelValidation();
            ValidationResult results = validation.Validate(model);
            if (!results.IsValid)
            {
                AjaxData.Success =false;
                AjaxData.Message =results.ToString("||");
                return Json(AjaxData);
            }
            #endregion

            #region 数据库验证
            string r = HttpContext.Session.GetString(R_KEY);
            r = r ?? "";
            var result = _baseUserService.ValidateUser(model.Account, model.Password, r);
            AjaxData.Success = result.Success;
            AjaxData.Message = result.Message;
            if (result.Success)
            {
                _authenticationService.signIn(result.Token, result.User.Account);
            }
            return Json(AjaxData);
            #endregion

        }
        [Route("GetCaptchaImage", Name = "BaseGetCaptchaImage")]
        public IActionResult GetCaptchaImage()
        {
            string captchaCode = CaptchaHelper.GenerateCaptchaCode();
            var result = CaptchaHelper.GetImage(116, 36, captchaCode);
            HttpContext.Session.SetString(CaptchaCodeSessionName, captchaCode);
            return new FileStreamResult(new MemoryStream(result.CaptchaByteData), "image/png");
        }
        private bool ValidateCaptchaCode(string userInputCaptcha)
        {
            var isValid = userInputCaptcha.Equals(HttpContext.Session.GetString(CaptchaCodeSessionName), StringComparison.OrdinalIgnoreCase);
            HttpContext.Session.Remove(CaptchaCodeSessionName);
            return isValid;
        }

        /// <summary>
        ///  根据帐号获取加盐密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [Route("getSalt", Name = "getSalt")]
        public IActionResult getSalt(string account)
        {
            var user = _baseUserService.GetUserInfoByAccount(account);
            if (user==null)
            {
                AjaxData.Success = false;
                AjaxData.Message = "未找到对应帐号";  
            }
            else
            {
                AjaxData.Success = true;
                AjaxData.Message = "帐号正确";
                AjaxData.Data = user.Salt;
            }    
            return Json(AjaxData);
        }
       
      
    }
}