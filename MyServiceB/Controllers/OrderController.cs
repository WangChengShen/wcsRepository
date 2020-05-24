using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MyServiceB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private IConfiguration configuration;
        public OrderController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public ActionResult Get()
        {
            string url = $"{configuration["ip"]}:{configuration["port"]}";

            return Ok($"Order From MyServiceB[{url}]");
        }
    }
}