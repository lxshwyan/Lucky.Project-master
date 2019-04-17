using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;    
using Lucky.Project.Web.Hub;
using Lucky.Project.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lucky.Project.Web.Controllers
{
    public class ChatDemoController : BaseController
    {
        private IHubContext<ChatHub> _hubContext;
        private IApplicationLifetime applicationLifetime;
        private ILogger<ChatDemoController> logger;
        private IConfiguration _configuration;

       

        public ChatDemoController(IHubContext<ChatHub> hubContext, IApplicationLifetime appLifetime,
            IConfiguration configuration,
            ILogger<ChatDemoController> logger)
        {
            this._hubContext = hubContext;
            this.applicationLifetime = appLifetime; 
            this.logger = logger;
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chat()
        {
            ViewData["SignalrURL"] = _configuration["SignalrURL"];
            return View();
        }
        [HttpGet("send")]
        public IActionResult Send()
        {
            ChatMessage message = new ChatMessage()
            {
                Type = 1,
                Content = $" {DateTime.Now}:->{new Random().Next(1, 10000)}",
                UserName = this.HttpContext.User.Identity.Name
            };
            _hubContext.Clients.All.SendAsync("Recv", message);
            return Json(this.AjaxData);
            
        }
        [HttpPost("SendToUser")]
        public async Task<IActionResult> SendToUser([FromBody] UserInfoViewModel model)
        {
            ChatMessage message = new ChatMessage()
            {
                Type = 1,
                Content = model.Content,
                UserName = this.HttpContext.User.Identity.Name
            };


            foreach (var item in  GetConnectionIds())
            {
                await this._hubContext.Clients.Client(item).SendAsync("Recv", message);
                    logger.LogInformation("SendToUser");
            }
            return Json(this.AjaxData);
        }


        [HttpPost("Group-Join")]
        public async Task<IActionResult> Join([FromBody] GroupViewModel model)
        {
            string userName = this.HttpContext.User.Identity.Name;

            foreach (var item in  GetConnectionIds())
            {
                await this._hubContext.Groups.AddToGroupAsync(item, model.Name);
            }  
             return Json(this.AjaxData);
        }


        [HttpPost("Group-Leave")]
        public async Task<IActionResult> Leave([FromBody] GroupViewModel model)
        {      
            foreach (var item in  GetConnectionIds())
            {
                await this._hubContext.Groups.RemoveFromGroupAsync(item, model.Name);
            } 
            return Json(this.AjaxData);
        }


        [HttpPost("SendToGroup")]
        public async Task<IActionResult> SendToGroup([FromBody] GroupChatMessage model)
        {
            ChatMessage message = new ChatMessage()
            {
                Type = 1,
                Content = model.Content,
                UserName = User.Identity.Name
            };   

            foreach (var item in  GetConnectionIds())
            {
                await this._hubContext.Clients.Group(model.GroupName).SendAsync(model.GroupName, message);
            }

             return Json(this.AjaxData);
        }

        [HttpGet("StopApp")]
        public IActionResult StopApp()
        {
            applicationLifetime.StopApplication();
            return new EmptyResult();
        }

        private List<string> GetConnectionIds()
        {
            string userName = this.HttpContext.User.Identity.Name;
            if (ChatHub.UserList.ContainsKey(userName))
            {
               return ChatHub.UserList[userName].ToList();
            }
            return new List<string>();
        }
    }
}