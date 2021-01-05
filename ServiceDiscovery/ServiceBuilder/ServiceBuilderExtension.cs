using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery
{
    public static class ServiceBuilderExtension
    {
        /// <summary>
        /// Core里面经常用到的建造者模式
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceBuilder CreateServiceBuilder(this IServiceProvider serviceProvider,
             Action<ServiceBuilder> config)
        {
            ServiceBuilder builder = new ServiceBuilder(serviceProvider);
            config(builder);
            return builder;
        }

    }
}
