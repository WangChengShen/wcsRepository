using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket.Models
{
    public class WebSocketMiddleWare
    {
        /*
         自定义中间件：
         1.具有ResquestDelegate参数的构造函数
         2.具有名为InvokeAsync或Invoke的方法，必须满足两个条件
            a.返回task
            b.类型为httpContext的参数
        */

        private RequestDelegate _next;
        public WebSocketMiddleWare(RequestDelegate resquestDelegate)
        {
            this._next = resquestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                System.Net.WebSockets.WebSocket socket = await httpContext.WebSockets.AcceptWebSocketAsync();

                ArraySegment<byte> arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes("服务器向客户端发消息"));

                var cancellationToken = new CancellationToken();
                await socket.SendAsync(arraySegment, System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);

                await _next(httpContext);
            }
            else
            {
                await httpContext.Response.WriteAsync("我不处理");
            }

        }
    }

    public static class WebSocketMiddleWareExstion
    {
        public static IApplicationBuilder UseWebSocket(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<WebSocketMiddleWare>();
        }
    }

}
