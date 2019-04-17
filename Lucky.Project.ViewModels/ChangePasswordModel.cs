/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.ViewModels
*文件名： ChangePasswordModel
*创建人： Lxsh
*创建时间：2019/3/25 13:28:51
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/25 13:28:51
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Project.ViewModels
{
   public class ChangePasswordModel
    {
        /// <summary>
        /// 当前登录token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 当前登录帐号
        /// </summary>
        public string Accont { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 重复密码
        /// </summary>
        public string NewPasswordRe { get; set; }
    }
}