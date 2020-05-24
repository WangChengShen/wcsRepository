using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    public class ConsulServiceProvider : IServiceProvider
    {
        private readonly ConsulClient _consulClient;
        public ConsulServiceProvider(string consulHost)
        {
            _consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(consulHost);
                //config.Datacenter = "dc1";
            });
        }
         
        public async System.Threading.Tasks.Task<List<string>> GetServicesAsync(string serviceName)
        {
            /*
             serviceName:服务组名称
             tag：同一组里面注册的时候可以添加tag信息，在这里根据tag进行搜索
             passingOnly：是否只要健康的服务
             */
            var queryResult = await _consulClient.Health.Service(serviceName, tag: "", passingOnly: true);
            List<string> serviceList = new List<string>();
            foreach (var item in queryResult.Response)
            {
                serviceList.Add($"{item.Service.Address}:{item.Service.Port}");
            }
            return serviceList;
        }


    }
}
