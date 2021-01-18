using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonCache
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(option =>
            {
                //添加Log4Net
                //引入Microsoft.Extensions.Logging.Log4Net.AspNetCore
                //在program里面配置注册，或则在StartUp文件里面注册
                option.AddLog4Net();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
