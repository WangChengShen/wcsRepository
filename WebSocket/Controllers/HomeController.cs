using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSocket.Models;

namespace WebSocket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// dotnet WebSocket.dll  --urls="http://*:6001" --ip="127.0.0.1" --port=6001 --weight=1
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ！！！注意：在Configure方法中添加 app.UseWebSockets();
        /// 
        /// core webSocket 此demo暂未实现，获取信息时老是报链接已关闭
        /// 不获取信息，仅发送信息没问题
        /// 
        /// </summary>
        public async void MyWebSocket(string name)
        {
            if (base.HttpContext.WebSockets.IsWebSocketRequest)
            {
                System.Net.WebSockets.WebSocket socket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                //保存起来
                SocketManger.AddSocket(name, Guid.NewGuid().ToString(), socket);

                CancellationToken token = new CancellationToken();
                string userMessage2 = await ReceiveStringAsync(socket);
                //while (socket.State == WebSocketState.Open)
                //{
                //    //ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
                //    //WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token); 
                //    //string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                //    string userMessage = await ReceiveStringAsync(socket);


                //    if (!string.IsNullOrEmpty(userMessage))
                //    {
                //        //if (result.MessageType == WebSocketMessageType.Close)
                //        //{

                //        //}
                //        //else
                //        //{
                //        SocketManger.SendOne(userMessage, token);
                //        //}
                //    }
                //} 
            }
            else
            {
                await HttpContext.Response.WriteAsync("我不处理");
            }
        }

        private async Task Echo(System.Net.WebSockets.WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
