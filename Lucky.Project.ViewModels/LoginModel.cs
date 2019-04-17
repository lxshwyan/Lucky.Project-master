/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.ViewModels
*文件名： LoginModel
*创建人： Lxsh
*创建时间：2019/3/20 16:42:06
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/20 16:42:06
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.ViewModels
{
   public class LoginModel
    {

        public string UserName { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string CaptchaCode { get; set; }
        public string Ip { get; set; }
        public string ReturnUrl { get; set; }
         public string R { get; set; }
    }
}