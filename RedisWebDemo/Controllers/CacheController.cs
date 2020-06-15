using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisWebDemo.Controllers
{
    public class CacheController : Controller
    {
        private readonly IDistributedCache distributedCache;
        public CacheController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            distributedCache.SetString("Hello", "Cache");
            distributedCache.GetString("Hello");
            return View();
        }
    }
}