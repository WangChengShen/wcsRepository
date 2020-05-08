using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common.Config
{
    public class SqlConnectionPool
    {
        public static string GetConnection(SqlConnecctionType sqlConnecctionType)
        {
            string connStr = string.Empty;
            switch (sqlConnecctionType)
            {
                case SqlConnecctionType.Read:
                    connStr = Dispatcher(ConfigurationManager.ReadConnString);
                    break;
                case SqlConnecctionType.Write:
                    connStr = ConfigurationManager.WriteConnString;
                    break;
                default:
                    throw new Exception("error sqlConnecctionType");
                    break;
            }
            return connStr;
        }

        private static int _Seed = 0;
         
        /// <summary>
        /// 调度分配--随机分配  
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private static string Dispatcher(string[] connStr)
        {
            string str = connStr[new Random(_Seed++).Next(0, connStr.Length)]; //平均策略
            str = connStr[_Seed++ % connStr.Length];//轮循，Seed需要考虑线程安全，加把锁
            //权重策略，需要在数据库字符串文件里面加点料，配置个权重比例参数；然后仿照微服务实现权重思路实现
            return str;
        }

        public enum SqlConnecctionType
        {
            Read,
            Write
        }
    }
}
