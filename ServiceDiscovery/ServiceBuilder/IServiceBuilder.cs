using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDiscovery
{

    /// <summary>
    /// 构建请求接口的地址
    /// </summary>
    public interface IServiceBuilder
    {
        //IServiceProvider ServiceProvider { get; set; }

        //string ServiceName { get; set; }

        //string UriScheme { get; set; }

        //ILoadBalancer LoadBalancer { get; set; }

        Task<Uri> BuildAsync(string path);
    }
}
