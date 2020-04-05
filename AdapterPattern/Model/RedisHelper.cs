using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern
{
    public class RedisHelper
    {
        public void AddRedis<T>()
        {
            Console.WriteLine("this is {0} add", this.GetType().Name);
        }

        public void UpdateRedis<T>()
        {
            Console.WriteLine("this is {0} update", this.GetType().Name);
        }

        public void QueryRedis<T>()
        {
            Console.WriteLine("this is {0} query", this.GetType().Name);
        }

        public void DeleteRedis<T>()
        {
            Console.WriteLine("this is {0} delete", this.GetType().Name);
        }
    }
}
