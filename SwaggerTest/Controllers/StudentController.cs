using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwaggerTest.Models;
using Wcs.BLL;
using Wcs.Models;

namespace SwaggerTest.Controllers
{
    [EnableCors("Any")]//设置允许跨域
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "group1")]
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

        /// <summary>
        /// core api model 验证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("SignUp")]
        [HttpPost]
        public WcsJosnResult SignUp([FromBody]SignUpReq req)
        {
            //不在方法里面进行验证，改到方法filter里面进行统一验证
            //if (!ModelState.IsValid)
            //    return new WcsJosnResult { Result = 0, Message = ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage };

            return new WcsJosnResult { Result = 1, Message = "已成功报名" };
        }

    }
}
