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
    /// <summary>
    /// 延迟式的sqlHelper,模式EF的SaveChane,一些列操作，但是只有最后SaveChange时才会入库
    /// 其实前面的一些列操作是把命令保存了下来，SaveChange时一起执行，可把事务带进来
    /// </summary>
    public class SqlHelperDelay : IDisposable //继承了IDisposable,使用时可使用using，使用过之后会执行Dispose方法，可在这个方法里面进行释放对象
    {
        public T Find<T>(int id) where T : Wcs.Models.BaseModel
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

        private IList<SqlCommand> commandList = new List<SqlCommand>();
        public void Insert<T>(T t) where T : Wcs.Models.BaseModel
        {
            Type type = typeof(T);
            SqlCommand comd = new SqlCommand();
            SqlParameter[] paraArray = type.GetPropertiesWithoutKey().Select(s => new SqlParameter($"@{s.GetMappingName()}", s.GetValue(t) ?? DBNull.Value)).ToArray();
            // 利用静态泛型缓存对sql进行缓存
            string sql = SqlBuilder<T>.GetInertSql();

            comd.CommandText = sql;
            comd.Parameters.AddRange(paraArray);
            this.commandList.Add(comd);
        }

        public void SaveChange()
        {
            if (commandList.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(SqlConnectionPool.GetConnection(SqlConnectionPool.SqlConnecctionType.Write)))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in commandList)
                            {
                                item.Connection = conn;
                                item.Transaction = trans;
                                item.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                        }
                        finally
                        {
                            commandList?.Clear();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            commandList?.Clear();
        }
    }
}
