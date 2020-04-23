using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Wcs.Common;

namespace ORMExplore.Model
{
    public class SqlHelper
    { 
        public static T Find<T>(int id)
        { 
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.myDBConnString))
            {
                conn.Open();
                Type type = typeof(T);
                SqlCommand comm = conn.CreateCommand();
                string para = type.GetProperties().Select(p => $"[{ p.Name}]").Aggregate((x, y) => x + "," + y);
                comm.CommandText = $" select {para} from {type.Name} where id={id}";

                var reader = comm.ExecuteReader();

                T t = default(T);
                if (reader.Read())
                {
                    t = (T)Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(t, reader[item.Name]);
                    }

                }
                return t; 
            }
        }

    }
}
