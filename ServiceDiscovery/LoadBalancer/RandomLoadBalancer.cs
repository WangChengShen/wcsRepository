using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    /// <summary>
    /// 随机
    /// </summary>
    public class RandomLoadBalancer : ILoadBalancer
    {
        private readonly Random random = new Random();
        public string Resolve(IList<string> services)
        {
            return services[random.Next(services.Count)];
        }
    }
}
