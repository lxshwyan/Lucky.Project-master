using FluentValidation;
using Lucky.Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Validation
{     
    public class LoginModelValidation : AbstractValidator<LoginModel>
    {
        public LoginModelValidation()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Account).NotEmpty().Length(4, 32).WithMessage("用户名不能为空且长度必须符合规则");
            RuleFor(x => x.Password).NotEmpty().Length(4, 32).WithMessage("密码不能为空且长度必须符合规则");
            RuleFor(x => x.CaptchaCode).NotEmpty().Length(4).WithMessage("请正确输入四位验证码");
         

        }
    }
}
