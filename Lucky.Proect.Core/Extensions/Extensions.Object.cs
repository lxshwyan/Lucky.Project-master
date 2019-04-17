/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Extensions
*文件名： Extensions
*创建人： Lxsh
*创建时间：2019/3/26 12:55:24
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 12:55:24
*修改人：Lxsh
*描述：
************************************************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Proect.Core.Extensions
{
    public static partial class Extensions
    {

        public static string ObjectToJSON(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

    }
}