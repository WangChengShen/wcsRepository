using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMq.SendDemo.Models
{
    public class MsgModel
    {
        public string LogType { get; set; }
        public string LogMsg { get; set; }
    }
}
