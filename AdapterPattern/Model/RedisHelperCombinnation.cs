using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern
{
    public class RedisHelperCombinnation : IDbHelper
    {
        private RedisHelper redisHelper = new RedisHelper();
        public RedisHelperCombinnation()
        { 
            Console.WriteLine("RedisHelperCombinnation 被构造");
        }

        public void Add<T>(T t)
        {
            redisHelper.AddRedis<T>();
        }

        public void Delete<T>(T t)
        {
            redisHelper.DeleteRedis<T>();
        }

        public void Query<T>(T t)
        {
            redisHelper.QueryRedis<T>();
        }

        public void Update<T>(T t)
        {
            redisHelper.UpdateRedis<T>();
        }
    }
}
