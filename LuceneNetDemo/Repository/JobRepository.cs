using LuceneNetDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
namespace LuceneNetDemo.Repository
{
    public class JobRepository
    {
        public static string connStr = "Data Source=TestDatabase1;Initial Catalog=gkBpo;User ID=gktestdbuser;Password=SS1592658589;timeout=14400;";
        public static List<Bpo_JobEntity> GetJobList(int count)
        {
            using (DbConnection conn = new SqlConnection(connStr))
            {
                string sql = $"select top {count} * from Bpo_Job where status=1 order by id desc";
                return conn.Query<Bpo_JobEntity>(sql).ToList();
            }
        }
    }
}