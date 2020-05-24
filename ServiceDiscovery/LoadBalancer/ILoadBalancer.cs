using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    /// <summary>
    /// 负载均衡的接口
    /// 不能的负载方式都继承这个接口来实现不同方式的负载
    /// </summary>
    public interface ILoadBalancer
    {
        string Resolve(IList<string> services);
    }
}
