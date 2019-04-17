/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Proect.Core.Cache
*文件名： NuLLCacheService
*创建人： Lxsh
*创建时间：2019/3/26 16:56:12
*描述
*=======================================================================
*修改标记
*修改时间：2019/3/26 16:56:12
*修改人：Lxsh
*描述：
************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Proect.Core.Cache
{
    public class NuLLCacheService : ICache
    {
        public void Dispose()
        {
          
        }

        public bool Exists(string key)
        {
            return false;
        }

        public T GetCache<T>(string key) where T : class
        {
            return default(T);
        }

        public void RemoveCache(string key)
        {
         
        }

        public void SetCache(string key, object value)
        {
            
        }

        public void SetCache(string key, object value, double expirationMinute)
        {
           
        }

        public void SetCache(string key, object value, DateTimeOffset expirationTime)
        {
           
        }

        public void SetSlidingCache(string key, object value, TimeSpan t)
        {
            
        }
    }
}