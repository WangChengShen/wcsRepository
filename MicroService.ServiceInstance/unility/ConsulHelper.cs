using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.ServiceInstance.unility
{
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dc1";
            });

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//命令行参数必须传入
            client.Agent.ServiceRegister(new AgentServiceRegistration
            {
                ID = $"Service_{Guid.NewGuid()}",//唯一标识
                Name = "WcsGroup",//组名称--GroupName
                Address = ip, //服务地址
                Port = port,//端口号
                Tags = new string[] { weight.ToString() },//标记，用来传自定义的参数，例如：传入权重参数
                Check = new AgentServiceCheck() //健康检查，心跳检测
                {
                    Interval = TimeSpan.FromSeconds(12),//间隔12秒
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60),//失败后多久移除单位秒，好像有最小值限制60s
                }
            });


        }
    }


}
