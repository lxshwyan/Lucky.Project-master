/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lucky.Project.Framework.InterceptorAttribute
*文件名： HystrixCommandAttribute
*创建人： Lxsh
*创建时间：2019/4/4 12:00:55
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/4 12:00:55
*修改人：Lxsh
*描述：
************************************************************************/
using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Project.Framework.InterceptorAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        public HystrixCommandAttribute(string fallBackMethod)
        {
            this.FallBackMethod = fallBackMethod;
        }

        public string FallBackMethod { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);//执行被拦截的方法
            }
            catch (Exception ex)
            {
                //context.ServiceMethod被拦截的方法。context.ServiceMethod.DeclaringType被拦截方法所在的类
                //context.Implementation实际执行的对象p
                //context.Parameters方法参数值
                //如果执行失败，则执行FallBackMethod
                var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(this.FallBackMethod);
                Object fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
                context.ReturnValue = fallBackResult;
                await Task.FromResult(0);
            }
        }
    }
}