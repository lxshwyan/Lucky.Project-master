/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Proect.Core.Models
*文件名： LoginModel
*创建人： Lxsh
*创建时间：2019/3/20 16:08:10
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/20 16:08:10
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Lucky.Project.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Lucky.Proect.Core.Models
{
    public class ValidLoginModel <T> where T:class 
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public T User { get; set; }
    }
}
