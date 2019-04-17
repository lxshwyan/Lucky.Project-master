using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucky.Project.Web.AOP;
using Microsoft.AspNetCore.Mvc;

namespace Lucky.Project.Web.Controllers
{
    public class AOPController : Controller
    {

        private Person p;
        
        public AOPController(Person p)
        {
            this.p = p;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            string s = await p.HelloAsync("yzk");
            return new string[] { "value1", "value2", s };
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}