using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributeStudy.Models
{
    public interface IMessageService
    {
        void Send();
    }
    public class SmsService : IMessageService
    {
        public void Send()
        {
            Console.WriteLine("Sms");
        }
    }

    public class EmailService : IMessageService
    {
        public void Send()
        {
            Console.WriteLine("Email");
        }
    }
}
