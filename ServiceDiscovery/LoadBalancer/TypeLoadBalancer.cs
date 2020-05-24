using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    public class TypeLoadBalancer
    {
        public static ILoadBalancer RandomLoad = new RandomLoadBalancer();
        public static ILoadBalancer RoundRobin = new RoundRoinLoadBalancer();
    }
}
