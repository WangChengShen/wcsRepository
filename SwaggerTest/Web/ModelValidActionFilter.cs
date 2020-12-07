using Microsoft.AspNetCore.Mvc.Filters;
using SwaggerTest.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerTest.Web
{
    /// <summary>
    /// 验证参数filter
    /// </summary>
    public class ModelValidActionFilter : Attribute, IActionFilter, IFilterMetadata
    {
        /// <summary>
        ///方法访问之后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 方法访问之前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string errMsg = string.Empty;
                if (context.ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().Exception != null)
                {
                    errMsg = context.ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().Exception.Message;
                    errMsg = $"ModelState Exception:{errMsg}";
                }
                else
                {
                    errMsg = context.ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage;
                }

                WcsJosnResult result = new WcsJosnResult()
                {
                    Result = 0,
                    Message = errMsg
                };

                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(result);
            }
        }
    }
}
