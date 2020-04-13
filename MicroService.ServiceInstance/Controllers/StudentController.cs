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
    /// <summary>
    /// 特性路由，如果直接访问http://127.0.0.1:6001/api/Student的话，不带方法，访问的是控制下面没有打Route特性路由的
    /// 接口，
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentBLL istudentBLL;

        public StudentController(IConfiguration configuration, ILogger<StudentController> logger, IStudentBLL istudentBLL)
        {
            this.configuration = configuration;
            _logger = logger;
            this.istudentBLL = istudentBLL;
        }

        [HttpGet]
        [Route("Get")]
        public WcsJosnResult<StudentModel> Get(int id)
        {
            return new WcsJosnResult<StudentModel>
            {
                Result = 1,
                Message = $"已成功获取，接口：{configuration["port"]}",
                Data = istudentBLL.GetById(id)
            };
        }

        [HttpGet]
        [Route("GetAll")]
        public WcsJosnResult<List<StudentModel>> Get()
        {
            return new WcsJosnResult<List<StudentModel>>
            {
                Result = 1,
                Message = $"已成功获取，接口：{configuration["port"]}",
                Data = istudentBLL.GetStudentList()
            }; 
        }


        [HttpGet]
        [Route("GetString")]
        public List<string> GetString()
        {
            _logger.LogWarning("this is only a test");
            return new List<string> {
            "A","B","C",configuration["port"],DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

    }
}