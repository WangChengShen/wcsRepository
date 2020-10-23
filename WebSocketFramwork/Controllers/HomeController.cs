using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using WebSocketFramwork.Models;

namespace WebSocketFramwork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        string _name = string.Empty;
        /// <summary>
        /// ！！！注意：在Configure方法中添加 app.UseWebSockets();
        /// </summary>
        public void MyWebSocket(string name)
        {
            if (HttpContext.IsWebSocketRequest)
            {
                _name = name;
                HttpContext.AcceptWebSocketRequest(handSocket);
            }
            else
            {
                Response.Write("我不处理");
            }
        }


        /// <summary>
        /// 无论是SuperSocket还是WebSocket都需要心跳包，需要健康检查，每隔多长时间去发送一条消息，浏览器定时发送消息给服务器，
        /// 服务器接收到某一条特定的消息之后，马上回复给浏览器，那就表示连接是正常的；否则就需要去重新连接，其实就是短信重连；
        /// </summary>
        /// <param name="aspNetWebSocketContext"></param>
        /// <returns></returns>
        public async Task handSocket(AspNetWebSocketContext aspNetWebSocketContext)
        {
            WebSocket socket = aspNetWebSocketContext.WebSocket;
            //保存起来
            SocketManger.AddSocket(_name, Guid.NewGuid().ToString(), socket);

            CancellationToken token = new CancellationToken();

            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token);

                string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                if (!string.IsNullOrEmpty(userMessage))
                {
                    //前端调用关闭事件
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        //如果是群聊的话，则要把该用户从Socket的list中移除
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,string.Empty,token);
                    }
                    else
                    {
                        SocketManger.SendOne(userMessage, token);
                    }
                }
            }
        }

    }
}