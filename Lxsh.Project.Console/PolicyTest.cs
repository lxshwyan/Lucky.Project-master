/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lxsh.Project.ConsoleTest
*文件名： PolicyTest
*创建人： Lxsh
*创建时间：2019/4/3 16:42:29
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/3 16:42:29
*修改人：Lxsh
*描述：
************************************************************************/
using Polly;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace Lxsh.Project.ConsoleTest
{
    public class PolicyTest
    {
        public static void Test1()
        {
            ISyncPolicy policy = Policy.Handle<ArgumentException>(ex => ex.Message == "年龄参数错误")
                .Or<Exception>(ex => ex.Message == "haha")  //或者
                .Fallback(() =>
                {
                    Console.WriteLine("出错了");
                });
            policy.Execute(() =>
            {
                //这里是可能会产生问题的业务系统代码
                Console.WriteLine("开始任务");
                throw new ArgumentException("年龄参数错误");
                throw new Exception("haha");
            });
        }
        public static void RetryForever()
        {
            try
            {
                ISyncPolicy policy = Policy.Handle<Exception>()
                //.WaitAndRetry(new TimeSpan[] { TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(2000) });
                .RetryForever();

                policy.Execute(() =>
                {
                    Console.WriteLine("开始任务");

                    //if (Environment.TickCount % 2 == 0)
                    if (DateTime.Now.Second % 10 != 0)
                    {
                        throw new Exception("出错");
                    }
                    Console.WriteLine("完成任务");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("未处理异常" + ex);
            }
        }
        public static void RetryNum()
        {
            try
            {
                ISyncPolicy policy = Policy.Handle<Exception>()
                //.WaitAndRetry(new TimeSpan[] { TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(2000) });
                .Retry(3);
                policy.Execute(() =>
                {
                    Console.WriteLine("Retry");
                    throw new Exception("出错");

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("未处理异常" + ex);
            }
        }
        public static void CircuitBreaker()
        {
            int i = 0;
            ISyncPolicy policy = Policy
           .Handle<Exception>()
           .CircuitBreaker(6, TimeSpan.FromSeconds(5));//连续出错6次之后熔断5秒(不会再去尝试执行业务代码）。

            while (true)
            {
                Console.WriteLine("开始Execute");
                try
                {
                    policy.Execute(() =>
                    {
                        i++;
                        Console.WriteLine("开始任务");
                        if (i<8)
                        {
                            throw new Exception("出错");  
                        }
                        
                        Console.WriteLine("完成任务");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("execute出错" + ex.GetType() + ":" + ex.Message);
                }
                Thread.Sleep(1000);
            }
        }

        public static void Wrap()
        {
            try
            {
                ISyncPolicy policyException = Policy.Handle<TimeoutRejectedException>()  //只针对超时异常
                    .Fallback(() =>
                    {
                        Console.WriteLine("fallback");
                    });
                ISyncPolicy policytimeout = Policy.Timeout(2, Polly.Timeout.TimeoutStrategy.Pessimistic);//乐观和悲观
                //ISyncPolicy policy3 = policyException.Wrap(policytimeout);//
                ISyncPolicy policy3 = Policy.Wrap(policyException, policytimeout);
                policy3.Execute(() =>
                {
                    Console.WriteLine("开始任务");
                   Thread.Sleep(5000);
                   // throw new Exception();
                    Console.WriteLine("完成任务");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("未处理异常" + ex.GetType() + ":" + ex.Message);
            }

        }

        public static void Catch()
        {
       
        }
    }
}