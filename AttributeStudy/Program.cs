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
            //    /*log4��ʹ��
            //     ���룺Microsoft.Extensions.Logging.Log4Net.A
            //     ���log4net.Config�����ļ�
            //     ��ӷ��������ַ�������pragram����startup����
            //     * ��һ�ַ�ʽ��pragram�������ConfigureLoggingȻ�����£�
            //     �ڶ�������Configure����ע��ILoggerFactory��Ȼ�����AddLog4Net����
            //     */

            //    logBuilder.AddLog4Net();
            //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //ConfigureLogging �Ƕ���־����������õģ�ClearProviders�����Ĭ�ϵ�����
                    //  webBuilder.ConfigureLogging(c => c.ClearProviders());

                    webBuilder.UseStartup<Startup>();

                    //�������д�Ļ�����������ͻ�ִ�� Startup+���������ļ��Ĵ��룬������StartupDevelopment.cs�ļ�
                    //webBuilder.UseStartup(Assembly.GetExecutingAssembly().FullName);
                });
    }
}
