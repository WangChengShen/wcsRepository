using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RedisDemo.DAL
{
    public class OrderDAL
    {
        public static string connStr = @"Data Source=.;Initial Catalog=Wcs.Db;User ID=sa;Password=123456;timeout=14400;";
        public static bool AddOrder(int productId,int userId, DateTime createTime)
        {
            using (SqlConnection conn = new SqlConnection(ProductDAL.connStr))
            {
                conn.Open();
                string sql = "insert into ProductOrder(ProductId,UserId,CreateTime) values(@ProductId,@UserId,@CreateTime);select @@IDENTITY";
                SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ProductId",productId),
                new SqlParameter("@CreateTime",createTime),
                new SqlParameter("@UserId",userId)
                };
                SqlCommand command = conn.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddRange(paras);
                var res = command.ExecuteScalar();
                return Convert.ToInt32(res ?? 0) > 0;
            }
        }
    }

}
