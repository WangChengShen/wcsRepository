using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    public class RoundRoinLoadBalancer : ILoadBalancer
    {
        private object _lock = new object();
        private int num = 0;
        public string Resolve(IList<string> services)
        {
            lock (_lock)
            {
                int index = num++ % services.Count;
                return services[index];
                 
                //if (_index >= services.Count)
                //{
                //    _index = 0;
                //}

                //return services[_index++];
            }
        }
    }
}
