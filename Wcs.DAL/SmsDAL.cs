using System;
  
namespace Wcs.DAL
{
    public class SmsDAL : IMessageDAL
    { 
        public void Send()
        {  
            Console.WriteLine("Sms");
        }

        

    }
}
