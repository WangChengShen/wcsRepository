﻿using System;
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
        /// 访问的时候需要在header里面添加
        /// Authorization参数，值=bearer+一个空格+token
        /// 如：bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRWxldmVuIiwiTmlja05hbWUiOiJFbGV2ZW4iLCJSb2xlIjoiQWRtaW5pc3RyYXRvciIsImV4cCI6MTU4Njk2NDUxMCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1NzI2IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1NzI2In0.vZNNPHWqqqFvTGamp0X4OvnbRZ4nyq4TofchugUugys
        /// 进行访问；
        /// 在postman里面可以直接选择bearer auth然后把token粘贴进去进行访问就可以了，
        /// 在ajax访问可以像图站点内图片bearer_autu.png一样进行访问
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