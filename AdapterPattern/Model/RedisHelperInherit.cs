using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern
{
    public class RedisHelperInherit : RedisHelper, IDbHelper
    {
        public void Add<T>(T t)
        {
            //调用父类RedisHelper的方法
            base.AddRedis<T>();
        }

        public void Delete<T>(T t)
        {
            //调用父类RedisHelper的方法
            base.DeleteRedis<T>();
        }

        public void Query<T>(T t)
        {
            //调用父类RedisHelper的方法
            base.QueryRedis<T>();
        }

        public void Update<T>(T t)
        {  //调用父类RedisHelper的方法
            base.UpdateRedis<T>();
        }
    }
}
