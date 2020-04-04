using System;
using Wcs.Models.Interface;

namespace Wcs.Models
{
    public class SmsService : IMessageService
    {
        public void Send()
        {
            Console.WriteLine("Sms");
        }
    }
}
