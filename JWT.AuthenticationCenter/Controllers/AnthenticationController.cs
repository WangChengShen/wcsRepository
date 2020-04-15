using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.AuthenticationCenter.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JWT.AuthenticationCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnthenticationController : ControllerBase
    { 
        private readonly ILogger<AnthenticationController> _logger; 
        private readonly IJWTService _iJWTService;
        private readonly IConfiguration _configuration;
        public AnthenticationController(ILogger<AnthenticationController> logger, IJWTService iJWTService, IConfiguration configuration)
        {
            _logger = logger;
            _iJWTService = iJWTService;
            _configuration = configuration;
        }

        [Route("Get")]
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return new List<int>() { 1, 2, 3, 4, 6, 7 };
        } 

        /// <summary>
        /// 访问此接口获取token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public string Login(string name, string password)
        {
            //模拟登陆
            if ("Eleven".Equals(name) && "123456".Equals(password))//应该数据库
            {
                //验证账号通过之后去获取token,然后返给客户端
                string token = this._iJWTService.GetToken(name);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            }
        }
    }
}
