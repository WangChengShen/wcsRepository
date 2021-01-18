using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonCache.Models
{
    /*
       自定义中间件：
       1.具有ResquestDelegate参数的构造函数
       2.具有名为InvokeAsync或Invoke的方法，必须满足两个条件
          a.返回task
          b.类型为httpContext的参数
      */
    public class ExceptionHandleMiddleware
    {
        private RequestDelegate _next;
        private ILogger<ExceptionHandleMiddleware> logger;
        public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //await httpContext.Response.WriteAsync("前面的业务逻辑");
            //await _next.Invoke(httpContext);
            //await httpContext.Response.WriteAsync("后面的业务逻辑");
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception);
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            string errorMsg =
                Environment.NewLine + $"【请求方式】:{context.Request.Method},"
              + Environment.NewLine + $"【当前Url】:{ context.Request.Path.ToString()},"
              + Environment.NewLine + $"【API错误信息】:{ exception.Message}"
              + Environment.NewLine + $"【InnerException】:{ exception.InnerException}"
              + Environment.NewLine + $"【StackTrace】:{ exception.StackTrace}"
              + Environment.NewLine + $"【Source】:{ exception.Source}"
              + Environment.NewLine + $"【TargetSite】:{ exception.TargetSite}"
              + Environment.NewLine + $"【GetBaseException】:{ exception.GetBaseException().ToString()}"
              + Environment.NewLine + "****************************************************************************************************************************************************************************";

            //记录日志
            logger.LogError(errorMsg);

            //context.Response.ContentType = context.Request.Headers["Accept"];

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
            {
                Result = 0,
                Message = exception.Message
            })).ConfigureAwait(false);

        }
    }

    public static class ExceptionHandleExtend
    {
        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }

}
