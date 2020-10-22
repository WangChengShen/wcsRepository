using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// </summary>
        public async void MyWebSocket(string name)
        {
            if (base.HttpContext.WebSockets.IsWebSocketRequest)
            {
                System.Net.WebSockets.WebSocket socket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                //保存起来
                SocketManger.AddSocket(name, Guid.NewGuid().ToString(), socket);

                CancellationToken token = new CancellationToken();

                while (socket.State == WebSocketState.Open)
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
                    WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token);

                    string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                    if (!string.IsNullOrEmpty(userMessage))
                    {
                        if (result.MessageType == WebSocketMessageType.Close)
                        {

                        }
                        else
                        {
                            SocketManger.SendOne(userMessage, token);
                        }
                    }
                }
            }
            else
            {
                await HttpContext.Response.WriteAsync("我不处理");
            }
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
