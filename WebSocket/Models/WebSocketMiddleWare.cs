using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
                string name = httpContext.Request.Query["name"].ToString();
                while (socket.State == WebSocketState.Open)
                {
                    //保存起来
                    SocketManger.AddSocket(name, Guid.NewGuid().ToString(), socket);

                    string userMessage = await ReceiveStringAsync(socket);

                    SocketManger.SendOne(userMessage, CancellationToken.None);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        //public async Task<string> ReceiveStringAsync(System.Net.WebSockets.WebSocket webSocket)
        //{
        //    var buffer = new ArraySegment<byte>(new byte[8192]);
        //    WebSocketReceiveResult result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
        //    while (!result.CloseStatus.HasValue)
        //    {
        //        //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

        //        result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
        //    }
        //}

        private async Task<string> ReceiveStringAsync(System.Net.WebSockets.WebSocket socket)
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.EndOfMessage)
            {
                result = await socket.ReceiveAsync(buffer, default(CancellationToken));
            }

            //var json = Encoding.UTF8.GetString(buffer.Array);
            //json = json.Replace("\0", "").Trim();
            //return json;

            //ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            //WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

            return userMessage;
        }
    }

    public static class WebSocketMiddleWareExstion
    {
        public static IApplicationBuilder UseWebSocketMiddleWare(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<WebSocketMiddleWare>();
        }
    }

}
