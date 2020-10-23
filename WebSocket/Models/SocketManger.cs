using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket.Models
{
    public class SocketManger
    {
        public static List<SocketModel> socketModels = new List<SocketModel>();

        public static void AddSocket(string name, string guid, System.Net.WebSockets.WebSocket socket)
        {
            if (socketModels.Count(s => s.Name == name) <= 0)
            {
                socketModels.Add(new SocketModel
                {
                    Name = name,
                    Guid = guid,
                    Socket = socket
                });
            }
        }

        public static void SendOne(string msg, CancellationToken cancellationToken)
        {
            string[] array = msg.Split(':');
            string toUser = array[0];
            string toMessage = array[1];

            SocketModel socket = socketModels.FirstOrDefault(s => s.Name == toUser);
            if (socket != null)
            {
                ArraySegment<byte> arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(toMessage));

                socket.Socket.SendAsync(arraySegment, System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);
            }  
        }


    }

    public class SocketModel
    {
        public string Name { get; set; }
        public string Guid { get; set; }

        public System.Net.WebSockets.WebSocket Socket { get; set; }
    }
}
