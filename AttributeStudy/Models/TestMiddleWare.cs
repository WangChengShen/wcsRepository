using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributeStudy.Models
{
    public class TestMiddleWare
    {
        /*
         自定义中间件：
         1.具有ResquestDelegate参数的构造函数
         2.具有名为InvokeAsync或Invoke的方法，必须满足两个条件
            a.返回task
            b.类型为httpContext的参数
        */
        private readonly RequestDelegate _next;

        public TestMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync("middleWare start");
            await _next(context);
            await context.Response.WriteAsync("middleWare end"); 
        }
    }

    public static class TestMiddleWareExtion
    { 
        public static IApplicationBuilder UserTest(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TestMiddleWare>();
        }
    }
}
