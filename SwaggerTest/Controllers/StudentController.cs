using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wcs.BLL;
using Wcs.Models;

namespace SwaggerTest.Controllers
{
    [EnableCors("Any")]//设置允许跨域
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentBLL istudentBLL;

        public StudentController(ILogger<StudentController> logger, IStudentBLL istudentBLL)
        {
            _logger = logger;
            this.istudentBLL = istudentBLL;
        }

        [ResponseCache(Duration = 600)]
        [HttpGet]
        [Route("GetData")]
        public int GetData()
        {
            return 1;
        }


        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public WcsJosnResult<StudentModel> GetById(int id)
        {
            return new WcsJosnResult<StudentModel>
            {
                Result = 1,
                Message = "成功获取",
                Data = istudentBLL.GetById(id)
            };
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetList")]
        public WcsJosnResult<List<StudentModel>> GetList()
        {
            return new WcsJosnResult<List<StudentModel>>
            {
                Result = 1,
                Message = "成功获取",
                Data = istudentBLL.GetStudentList()
            };
        }


    }
}
