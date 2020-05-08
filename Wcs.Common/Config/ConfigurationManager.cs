using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wcs.Common
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class ConfigurationManager
    {
        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build(); ;
            WriteConnString = configuration["ConnectionStrings:Write"];
             
            ReadConnString = configuration.GetSection("ConnectionStrings").GetSection("Read").GetChildren()
                .Select(s => s.Value).ToArray();
        }

        public static string WriteConnString
        {
            get;
        }
        public static string[] ReadConnString
        {
            get;
        }

}
}
