using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiddleWareFlow
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class ConfigurationManager
    { 
        private static IConfigurationRoot _Configuration;
        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            _Configuration = builder.Build();
        }

        public static string GetNote(string noteName)
        {
            return _Configuration[noteName];
        }
    }
}
