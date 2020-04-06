using System;
using System.Collections.Generic;
using System.Text; 

namespace Wcs.DAL
{
    public class EmailDAL : IMessageDAL
    {
        public void Send()
        {
            Console.WriteLine("Email");
        }
    }
}
