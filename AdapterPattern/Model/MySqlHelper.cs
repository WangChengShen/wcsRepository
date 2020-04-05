using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern
{
    public class OracleHelper : IDbHelper
    {
        public void Add<T>(T t)
        {
            Console.WriteLine("OracleHelper 添加数据");
        }

        public void Delete<T>(T t)
        {
            Console.WriteLine("OracleHelper 删除数据");
        }

        public void Query<T>(T t)
        {
            Console.WriteLine("OracleHelper 查询数据");
        }

        public void Update<T>(T t)
        {
            Console.WriteLine("OracleHelper 修改数据");
        }
    }
}
