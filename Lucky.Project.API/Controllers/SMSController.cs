using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lucky.Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        // GET api/values
        [HttpGet("Send")]
        public bool Send(string msg)
        {
            string value = Request.Headers["X-Hello"];
            Console.WriteLine($"x-hello={value}");
            Console.WriteLine("发送短信" + msg);
            return true;
        }
    }
}