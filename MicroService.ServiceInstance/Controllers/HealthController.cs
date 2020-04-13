using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wcs.BLL;
using Wcs.Models;

namespace MicroService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly ILogger<HealthController> _logger;

        public HealthController(IConfiguration configuration, ILogger<HealthController> logger)
        {
            this.configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// 健康检查，要返回200或200+的状态码，访问验证时是空白，在监控里面可以看到返回200状态码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is HealthController  {this.configuration["port"]} Invoke");

            return Ok(); 
        }

         

    }
}