using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulServiceRegistration
{
    /// <summary>
    /// 把注册Consule服务独立封装出来，以方便每个接口服务使用；
    /// core框架使用组件，一般在service里面先Add，然后在Configure里面Use
    /// 按照这个规则进行封装
    /// </summary>
    public static class ConsulRegistrationExtensions
    {
        public static void AddConsule(this IServiceCollection serviceCollection)
        {
            //引入Microsoft.Extensions.Hosting包
            // 读取服务配置文件
            var config = new ConfigurationBuilder().AddJsonFile("service.config.json").Build();

            //读取配置到ConsulServiceOptions实体，并注册为服务，使用时可以注入的方式使用
            serviceCollection.Configure<ConsulServiceOptions>(config);
        }

        public static IApplicationBuilder UseConsule(this IApplicationBuilder app, IConfiguration configuration)
        {
            //获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            // 获取服务配置项
            IOptions<ConsulServiceOptions> consulServiceOptions = app.ApplicationServices.GetRequiredService<IOptions<ConsulServiceOptions>>();

            ConsulServiceOptions serviceOptions = consulServiceOptions.Value;

            // 服务ID必须保证唯一
            serviceOptions.ServiceId = Guid.NewGuid().ToString();

            var consulClient = new ConsulClient(configuration =>
            {
                //服务注册的地址，集群中任意一个地址
                configuration.Address = new Uri(serviceOptions.ConsulAddress);
                // configuration.Datacenter = "dc1";//数据中心的名称
            });


            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//命令行参数必须传入

            //注册为服务，并设置参数
            consulClient.Agent.ServiceRegister(new AgentServiceRegistration
            {
                ID = serviceOptions.ServiceId,//唯一标识
                Name = serviceOptions.ServiceName,//组名称--GroupName
                Address = ip, //服务地址
                Port = port,//端口号
                Tags = new string[] { weight.ToString() },//标记，用来传自定义的参数，例如：传入权重参数
                Check = new AgentServiceCheck() //健康检查，心跳检测
                {
                    Interval = TimeSpan.FromSeconds(12),//间隔12秒
                    HTTP = $"http://{ip}:{port}{serviceOptions.HealthCheck}",
                    Timeout = TimeSpan.FromSeconds(5),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60),//失败后多久移除单位秒，好像有最小值限制60s
                }
            }).Wait();

            // 应用程序终止时，立即注销服务，而不是等待健康监测出来后移除
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceOptions.ServiceId).Wait();
            });

            return app;

        }


    }
}
