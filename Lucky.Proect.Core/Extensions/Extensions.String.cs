/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Proect.Core.Extensions
*文件名： Extensions
*创建人： Lxsh
*创建时间：2019/3/27 11:07:20
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/27 11:07:20
*修改人：Lxsh
*描述：
************************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lucky.Proect.Core.Extensions
{
   public static partial   class Extensions
    {
          [DebuggerStepThrough] //该特性是用在方法前面的，在想要跳过的方法前面加上 
        public static T FromJson<T>(this string jsonStr)
        {
            return string.IsNullOrEmpty(jsonStr) ? default(T) : JsonConvert.DeserializeObject<T>(jsonStr);
        }
        /// <summary>
        /// 指示指定的字符串是 null、空或者仅由空白字符组成。
        /// </summary>
        [DebuggerStepThrough] //该特性是用在方法前面的，在想要跳过的方法前面加上 
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}