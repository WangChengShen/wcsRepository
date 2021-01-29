using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RedisDemo.DAL
{
    public class ProductDAL
    {
        //public static string connStr= @"Data Source=DESKTOP-GCL6M23\WCSSQL;Initial Catalog=Wcs.Db;User ID=sa;Password=123456;timeout=14400;";
        public static string connStr = @"Data Source=.;Initial Catalog=Wcs.Db;User ID=sa;Password=123456;timeout=14400;";
        public static int GetProductStock()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select top 1 stock from Product ";
                int reader = Convert.ToInt32(comm.ExecuteScalar());
                return reader;
            }
        }

        public static bool UpdateProductStock()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = "update Product set Stock=Stock-1  ";
                return comm.ExecuteNonQuery() > 0;
            }
        }
        public static bool UpdateProductStock(long stock)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = $"update Product set Stock={stock}  ";
                return comm.ExecuteNonQuery() > 0;
            }
        }
    }

   

}
