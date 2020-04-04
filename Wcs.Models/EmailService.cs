using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Models.Interface;

namespace Wcs.Models
{
    public class EmailService : IMessageService
    {
        public void Send()
        {
            Console.WriteLine("Email");
        }
    }
}
