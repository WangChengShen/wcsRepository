using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /*
        当过滤器构造函数中要传入参数时，此时特殊处理，有四种方式：
            1.此过滤器全局注册，不要加到Controller和方法上面
            2.使用 ServiceFilter 特性 (记得在startUp的ConfigureService注册下CustomExceptionFilterAttribute)
            3.使用TypeFilter 特性（此特性不用在ConfigureService里面进行注册）
            4.IFilterFactory 通过集成接口自己实现，稍微麻烦（和ServiceFilter原理差不多，实现请看CustomFilterFactoryAttribute，也要在startUp的ConfigureService注册下CustomExceptionFilterAttribute）（简单理解，有点复杂，不太推荐）

            ServiceFilter是第4种的封装，TypeFilter在此基础又进一步，但本质上2,3,4是一样的
        */
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;
        private readonly IModelMetadataProvider modelMetadataProvider;
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger,
            IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            this.modelMetadataProvider = modelMetadataProvider;
        }


        /// <summary>
        /// 当发生异常时会进来
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled) //如果异常没有被处理
            {
                //设置Console 输出汉字编码
                //Console.OutputEncoding = System.Text.Encoding.UTF8;
                //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                Console.WriteLine($"路径：{context.HttpContext.Request.Path}，错误信息：{context.Exception.Message}");

                _logger.LogError($"路径：{context.HttpContext.Request.Path}，错误信息：{context.Exception.Message}");

                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.Result = new JsonResult(new
                    {
                        Result = false,
                        Message = "发生错误，请联系管理员"
                    });
                }
                else
                {
                    // context.Result = new RedirectResult("/Views/Shared/Error.cshtml"); //直接跳转页面

                    //把错误信息传到页面里面
                    var result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState);
                    result.ViewData.Add("Exception", context.Exception); 
                    context.Result = result; 
                }
                context.ExceptionHandled = true;//处理过了设置为true 
            }
            // base.OnException(context);去掉
        }

        /// <summary>
        /// 异步版本
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public override Task OnExceptionAsync(ExceptionContext context)
        //{
        //    return base.OnExceptionAsync(context);
        //}


        /// <summary>
        /// 在core里面没有封装判断是否是Ajax的请求方法，自己封装一下，Ajax的请求是XmlHttpRequest发起的，
        /// 只需要判断头部X-Requested-with是否等于XMLHttpRequest即可
        /// </summary> 
        /// <returns></returns>
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-with"];
            return "XMLHttpRequest".Equals(header);
        }
    }
}
