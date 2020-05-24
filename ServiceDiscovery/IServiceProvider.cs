using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDiscovery
{
    public interface IServiceProvider
    {
        Task<List<string>> GetServicesAsync(string serviceName);
    }
}
