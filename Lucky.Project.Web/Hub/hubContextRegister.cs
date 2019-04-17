using Lucky.Proect.Core;
using Lucky.Proect.Core.Extensions;
using Lucky.Proect.Core.RabbitMQ;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucky.Project.Web.Hub
{
    public class hubContextRegister : IhubContextRegister
    {
        private ConfigOption _dbOption;
        IHubContext<ChatHub> _chatHub;
        public hubContextRegister(IHubContext<ChatHub> chatHub,IOptionsSnapshot<ConfigOption> options)
        {
             _dbOption = options.Get("LuckyConfig");
            _chatHub = chatHub;
        }
        public void InitMessageRegister()
        {
          
            string strRabbitMQconfig =_dbOption.RabbitMQconfig;
            MQHelper tMQHelper = MQHelperFactory.CreateBus(strRabbitMQconfig);

            #region 模拟第三方消息发送
            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        chatHub.Clients.All.SendAsync("Notify", $" {DateTime.Now}:->{new Random().Next(1, 10000)}");
            //        Task.Delay(5000);
            //    }
            //});
            int i = 0;
            string strErrorMsg = "";
            Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    i++;
                    var bodyMsg = new SystemMessage()
                    {
                        Content = $"定时第{i}条信息",
                        DateTime = DateTime.Now.ToString(),
                        Title = "四方博瑞安防平台信息",
                        Type = "log".ObjectToJSON()
                    };
                    var message = new RabbitMQMsg { Body = bodyMsg.ObjectToJSON(), Category = CategoryMessage.System, Dst = "预留字段", SendTime = DateTime.Now.ToString(), Src = "消息来源" };

                    tMQHelper.TopicPublish(message.Category.ToString() + ".xA", message, ref strErrorMsg);
                    Task.Delay(5000);

                };
            });
            #endregion

            #region 通过signar发送到客户端
            tMQHelper.TopicSubscribe(Guid.NewGuid().ToString(), s =>
          {
              Console.WriteLine("当前收到信息：" + s.Body.FromJson<SystemMessage>().Content);


              foreach (var connection in ChatHub.UserList)
              {

                  if (_chatHub != null && connection.Key != null)
                  {
                      _chatHub.Clients.Client(connection.Key).SendAsync("Recv", "当前收到信息：" + s.Body.FromJson<SystemMessage>().Content);
                  }
              }
              // chatHub.Clients.All.SendAsync("Notify", $" {DateTime.Now}:->{new Random().Next(1, 10000)}");
          }, true, CategoryMessage.System.ToString() + ".*", CategoryMessage.Alarm.ToString() + ".*");
            #endregion   
        }
    }
}
