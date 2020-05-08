using Newtonsoft.Json;
using ORMExplore.unility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Wcs.Common;
using Wcs.Common.Config;

namespace ORMExplore
{
    public class SqlHelper
    {
        public static T Find<T>(int id) where T : Wcs.Models.BaseModel
        {
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.WriteConnString)) 
            using (SqlConnection conn = new SqlConnection(SqlConnectionPool.GetConnection(SqlConnectionPool.SqlConnecctionType.Read)))
            {
                conn.Open();
                Type type = typeof(T);
                SqlCommand comm = conn.CreateCommand();

                //var attribute = type.GetCustomAttributes(typeof(WcsORMTableNameAttribute), true);

                //string tableName = string.Empty;
                //if (attribute != null && attribute.Length > 0)
                //    tableName = ((WcsORMTableNameAttribute)attribute.First()).TableName;

                string tableName = type.GetMappingName();
                //利用静态泛型缓存对sql进行缓存
                comm.CommandText = $"{SqlBuilder<T>.GetFindSql()} {id}";

                var reader = comm.ExecuteReader();

                T t = default(T);
                if (reader.Read())
                {
                    t = (T)Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(t, reader[item.GetMappingName()]);
                    }
                }
                return t;
            }
        }

        public static int Insert<T>(T t) where T : Wcs.Models.BaseModel
        {
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.WriteConnString))
            using (SqlConnection conn = new SqlConnection(SqlConnectionPool.GetConnection(SqlConnectionPool.SqlConnecctionType.Write)))
            {
                conn.Open();
                Type type = typeof(T);
                SqlCommand comd = conn.CreateCommand();

                //扩展方法GetPropertiesWithoutKey把主键过滤掉
                //string propString = type.GetPropertiesWithoutKey().Select(s => s.GetMappingName()).Aggregate((x, y) => x + "," + y);
                //string valueString = type.GetPropertiesWithoutKey().Select(s => $"@{s.GetMappingName()}").Aggregate((x, y) => x + "," + y);
                //string sql = $@" insert into {type.GetMappingName()}({propString}) values({valueString});select @@identity;";

                SqlParameter[] paraArray = type.GetPropertiesWithoutKey().Select(s => new SqlParameter($"@{s.GetMappingName()}", s.GetValue(t) ?? DBNull.Value)).ToArray();
                // 利用静态泛型缓存对sql进行缓存
                string sql = SqlBuilder<T>.GetInertSql();

                comd.CommandText = sql;
                comd.Parameters.AddRange(paraArray);
                return Convert.ToInt32(comd.ExecuteScalar());
            }
        }

        public static bool Delete<T>(int id) where T : Wcs.Models.BaseModel
        {
            // using (SqlConnection conn = new SqlConnection(ConfigurationManager.WriteConnString))
            using (SqlConnection conn = new SqlConnection(SqlConnectionPool.GetConnection(SqlConnectionPool.SqlConnecctionType.Write)))
            {
                conn.Open();
                Type type = typeof(T);
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = $"{SqlBuilder<T>.GetFindSql()} {id}";
                return comm.ExecuteNonQuery() > 0;
            }
        }

        public static bool Update<T>(T t) where T : Wcs.Models.BaseModel
        {
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.WriteConnString))
            using (SqlConnection conn = new SqlConnection(SqlConnectionPool.GetConnection(SqlConnectionPool.SqlConnecctionType.Write)))
            {
                conn.Open();
                Type type = typeof(T);
                SqlCommand comm = conn.CreateCommand();
                int id = (int)type.GetProperty("Id").GetValue(t);

                SqlParameter[] paraArray = type.GetProperties().Select(s => new SqlParameter($"@{s.GetMappingName()}", s.GetValue(t) ?? DBNull.Value)).ToArray();
                comm.CommandText = $"{SqlBuilder<T>.GeUpdateSql()} {id}";
                comm.Parameters.AddRange(paraArray);
                return comm.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 按需更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Update<T>(int Id, string json) where T : Wcs.Models.BaseModel
        {
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.WriteConnString))
            using (SqlConnection conn = new SqlConnection(SqlConnectionPool.GetConnection(SqlConnectionPool.SqlConnecctionType.Write)))
            { 
                Type type = typeof(T);
                T t = JsonConvert.DeserializeObject<T>(json);

                string tableName = type.GetMappingName();
                string sqlUpdata = $@" update {tableName} set {type.GetPropertiesInJson(json).Select(prop => $"{prop.GetMappingName()}=@{prop.GetMappingName()}").Aggregate((x, y) => x + "," + y)}
                            where Id=@Id";

                SqlParameter[] paraArray = type.GetPropertiesInJson(json)
                    .Select(s => new SqlParameter($"@{s.GetMappingName()}", s.GetValue(t) ?? DBNull.Value))
                    .Append(new SqlParameter("@Id", Id))
                    .ToArray();

                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = sqlUpdata;
                comm.Parameters.AddRange(paraArray);
                return comm.ExecuteNonQuery() > 0;
            }
        }





    }
}
