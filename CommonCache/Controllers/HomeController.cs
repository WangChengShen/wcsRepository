using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CommonCache.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace CommonCache.Controllers
{

    public class HomeController : Controller
    {

        #region Nginx 反向代理缓存 config
        //proxy_cache_path /zhaoxi/support/nginx-1.17.8/data levels=1:2 keys_zone=web_cache:50m inactive=1m max_size=1g;

        /*
         * 
          location /third/ {
                proxy_store off;
                proxy_redirect off;
                proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header X-Real-IP $remote_addr;
                proxy_set_header Host $http_host;
                proxy_pass http://AspNetCore31/third/;
            
                # 缓存使用前面定义的内存区域
                proxy_cache web_cache;
                # 对于 200 和 304 的响应码进行缓存，过期时间为 2 分钟，这会覆盖前面定  义的    1  分钟过期时间
                proxy_cache_valid 200 304 2m;
                # 设置缓存的 key，直接用URL
                proxy_cache_key  $scheme$proxy_host$request_uri;
             }
          */
        #endregion

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration iconfiguration;
        private IMemoryCache imemoryCache;
        private IDistributedCache _iDistributedCache;
        public HomeController(ILogger<HomeController> logger, IConfiguration iconfiguration, IMemoryCache imemoryCache, IDistributedCache iDistributedCache)
        {
            _logger = logger;
            this.iconfiguration = iconfiguration;
            this.imemoryCache = imemoryCache;
            _iDistributedCache = iDistributedCache;
        }
        /// <summary>
        /// dotnet CommonCache.dll  --urls="http://*:6001" --ip="127.0.0.1" --port=6001 
        /// </summary>
        /// <returns></returns>
        //[ResponseCache(Duration =600)] //
        public IActionResult Index()
        {
            //在Response.Headers里面添加Cache-Control和ResponseCache的效果是一样的，ResponseCache就是这样封装的
            //base.HttpContext.Response.Headers["Cache-Control"] = "public,max-age=600";
            ViewBag.RequestUrl = $"{this.Request.Scheme}://{Request.Host}:{ iconfiguration["port"]}";
            ViewBag.ActionTime = DateTime.Now;

            #region MemoryCache 服务器本地缓存 
            //MemoryCache,新的语法糖：out出的参数可以不用声明，在下面直接使用
            if (imemoryCache.TryGetValue<string>("memoryCacheKey", out string outVal))
            {
                ViewBag.MemoryCache = outVal;
            }
            else
            {
                outVal = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                imemoryCache.Set("memoryCacheKey", outVal, TimeSpan.FromSeconds(120));
                ViewBag.MemoryCache = outVal;
            }
            #endregion

            #region 分布式缓存--解决缓存在不同实例共享问题
            string key = $"HomeController-Info";
            {
                string time = this._iDistributedCache.GetString(key);
                if (!string.IsNullOrWhiteSpace(time))
                {

                }
                else
                {
                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                    this._iDistributedCache.SetString(key, time, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120)
                    });
                }
                base.ViewBag.DistributedCacheNow = time;
            }
            #endregion

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
