using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AttributeStudy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //.ConfigureLogging(logBuilder =>
            //{
            //    /*log4的使用
            //     引入：Microsoft.Extensions.Logging.Log4Net.A
            //     添加log4net.Config配置文件
            //     添加服务有两种方法，在pragram或在startup里面
            //     * 第一种方式在pragram配置添加ConfigureLogging然后如下，
            //     第二种是在Configure里面注入ILoggerFactory，然后调用AddLog4Net方法
            //     */

            //    logBuilder.AddLog4Net();
            //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //ConfigureLogging 是对日志组件进行配置的，ClearProviders是清除默认的配置
                    //  webBuilder.ConfigureLogging(c => c.ClearProviders());

                    webBuilder.UseStartup<Startup>();

                    //如果这样写的话，则接下来就会执行 Startup+环境标量文件的代码，现在是StartupDevelopment.cs文件
                    //webBuilder.UseStartup(Assembly.GetExecutingAssembly().FullName);
                });
    }
}
