using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Wcs.MicroService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IConfiguration _iConfiguration;
        public TestController(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is TestController  {this._iConfiguration["port"]} Invoke");

            return new JsonResult(new
            {
                message = "This is TestControllerIndex",
                Port = this._iConfiguration["port"],
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")
            });
        }

        /// <summary>
        /// 打了Authorize特性，需要验证token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("IndexA")]
        public IActionResult IndexA()
        {
            Console.WriteLine($"This is TestController  {this._iConfiguration["port"]} Invoke");

            return new JsonResult(new
            {
                message = "This is TestControllerIndex",
                Port = this._iConfiguration["port"],
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")
            });
        }
    }
}