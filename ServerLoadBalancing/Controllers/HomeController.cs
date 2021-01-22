using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServerLoadBalancing.Models;

namespace ServerLoadBalancing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        /// <summary>
        /// Nginx负载均衡（nignx的配置文件配成站点下的nginx.conf文件）（启动）
        /// Nginx for windows  官网：http://nginx.org/en/download.html
        /// 到Nginx的目录下面cmd进入到命令行模式：
        /// 启动Nginx命令：start nignx.exe 
        /// 杀死Nginx进程：taskkill /IM  nginx.exe  /F
        /// *******************************************************************
        /// 把站点启动三个实例，分别是：5726，5727，5728
        /// 然后配置Nginx配置负载均衡转发到这三个站点实例上面，通过观察页面的地址和参数看结果
        /// dotnet ServerLoadBalancing.dll --urls="http://*:5726" --ip="127.0.0.1" --port="5726"
        /// dotnet ServerLoadBalancing.dll --urls="http://*:5727" --ip="127.0.0.1" --port="5727"
        /// dotnet ServerLoadBalancing.dll --urls="http://*:5728" --ip="127.0.0.1" --port="5728"
        /// </summary>
        private static int totalCount = 0;
        public IActionResult Index()
        {
            ViewBag.BrowserUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}";
            ViewBag.InternalUrl = $"{Request.Scheme}://{Request.Host.Host}:{this.configuration["port"]}";
            ViewBag.TotalCount = totalCount++;

            //负载均衡登录用户的保存，因为负载均衡了，所以会造成用户信息保存数据共享的问题，解决套路
            /*
             1.配置nigx（ip_hash）为会话粘滞，这样来自同一个ip的客户端会把请求转到同一个服务实例上面，这样可以解决，但是不推荐
             2.session共享 (一种实现用Redis) nuget引入 Microsoft.Extensions.Caching.Redis
             3.请求携带，把账号密码放到cookie返给客户端（不推荐不安全）
             4.JTW/IdentityService4,token模式
             */
           
            string user = this.HttpContext.Session.GetString("CurrentUser");
            if (string.IsNullOrEmpty(user))
            {
                HttpContext.Session.SetString("CurrentUser",$"WCS-{this.configuration["port"]}");

                user = $"WCS-{this.configuration["port"]}";
            }

            ViewBag.CurrentUser = user; 
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
