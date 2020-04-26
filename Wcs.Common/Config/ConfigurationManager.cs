using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

            IConfigurationRoot configuration = builder.Build();
            WcsDBConnString = configuration["Wcs.Db"];
        }

        public static string WcsDBConnString
        {
            get;
        }

    }
}
