using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreFilterStudy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(configureLogging =>
            {
                //���˵���־��𣬺͵ȼ�
                configureLogging.AddFilter("Microsoft", LogLevel.Warning);
                configureLogging.AddFilter("System", LogLevel.Warning);

                //configureLogging.AddFilter("Microsoft", LogLevel.Information);
                //configureLogging.AddFilter("System", LogLevel.Information);

                //���Microsoft.Extensions.Logging.Log4Net.AspNetCore��
                configureLogging.AddLog4Net();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
