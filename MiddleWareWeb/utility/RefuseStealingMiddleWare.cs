using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleWareWeb.utility
{
    /*
     自定义中间件：
     1.具有ResquestDelegate参数的构造函数
     2.具有名为InvokeAsync或Invoke的方法，必须满足两个条件
        a.返回task
        b.类型为httpContext的参数
    */
    public class RefuseStealingMiddleWare
    {
        private readonly RequestDelegate _next;
        public RefuseStealingMiddleWare(RequestDelegate requestDelegate)
        {
            this._next = requestDelegate;
        }

        /// <summary>
        /// 1.此方法执行自己的逻辑
        /// 2.执行下一个中间价Next
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            string url = httpContext.Request.Path.Value;
            if (!url.Contains("jpg")) //在这里仅演示jpg图片的防盗链
            {
                await _next(httpContext);//
            }

            string urlReferrer = httpContext.Request.Headers["Referer"];
            if (string.IsNullOrEmpty(urlReferrer))
            {
                await SetForbiddenImage(httpContext); //设置404图片
            }
            else if (!urlReferrer.Contains("localhost")) //非当前域名
            {
                await SetForbiddenImage(httpContext);//设置404图片
            }
            else
            {
                await _next(httpContext); //验证通过则执行下一个中间件
            }
        }

        public async Task SetForbiddenImage(HttpContext httpContext)
        {
            string defaultImagePath = "wwwroot/img/null.jpg";
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), defaultImagePath);
            FileStream fs = File.OpenRead(path);
            byte[] bytes = new byte[fs.Length];
            await fs.ReadAsync(bytes, 0, bytes.Length);
            await httpContext.Response.Body.WriteAsync(bytes,0, bytes.Length);
        }

    }

    /// <summary>
    /// 对IApplicationBuilder进行扩展
    /// </summary>
    public static class RefuseStealingMiddleWareExition
    {
        public static IApplicationBuilder UseRefuseStealing(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RefuseStealingMiddleWare>();
        }
    }
}
