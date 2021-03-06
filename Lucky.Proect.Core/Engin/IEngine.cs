﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lucky.Proect.Core
{
    public interface IEngine
    {
        /// <summary>
        /// 构建一个实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;
        /// <summary>
        /// 构建类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);
    }
}
