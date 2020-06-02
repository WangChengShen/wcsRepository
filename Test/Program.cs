using ServiceDiscovery;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 读取服务

            //ConsulServiceProvider consulServiceProvider = new ConsulServiceProvider("http://127.0.0.1:8500");
            //string serviceName = "MyServiceA";
            //Task<List<string>> serviceList = consulServiceProvider.GetServicesAsync(serviceName);

            //foreach (var item in serviceList.Result)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion


            #region 生成接口地址并访问
            //生成 ServiceBuilder方式1：
            //ServiceBuilder serviceBuilder = new ServiceBuilder()
            //{
            //    LoadBalancer = new RoundRoinLoadBalancer(),
            //    ServiceName = "MyServiceA",
            //    ServiceProvider = new ConsulServiceProvider("http://127.0.0.1:8500"),
            //    UriScheme = Uri.UriSchemeHttp
            //};

            //生成 ServiceBuilder方式2（用建造者模式，比较优雅，推荐）：
            var seviceProvider = new ConsulServiceProvider("http://127.0.0.1:8500");
            var serviceBuilder = seviceProvider.CreateServiceBuilder(builder =>
            {
                builder.ServiceName = "MyServiceA";
                builder.LoadBalancer = TypeLoadBalancer.RandomLoad;// new RoundRoinLoadBalancer();
                builder.UriScheme = Uri.UriSchemeHttp;
            });

            HttpClient httpClient = new HttpClient();
            for (int i = 0; i < 5; i++)
            {
                Task<Uri> url = serviceBuilder.BuildAsync("/Api/Health");
                Uri apiUrl = url.Result;

                Console.WriteLine($"----------------第{i}次请求-----------------------");
                Console.WriteLine($"{DateTime.Now}-正在调用{apiUrl}");

                var content = httpClient.GetStringAsync(apiUrl).Result;
                Console.WriteLine($"调用结果：{content}");

                Task.Delay(1000).Wait();
            }

            #endregion

            Console.WriteLine("Hello World!");
        }
    }
}
