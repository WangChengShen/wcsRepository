using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreFilterStudy.Models;
using CoreFilterStudy.Filter;

namespace CoreFilterStudy.Controllers
{
    /// <summary>
    /// Core里面过滤器的有5种:
    /// 
    /// AuthorizationFilter
    /// ResourchFilter
    /// ExceptionFilter （报异常是才会执行）
    /// ActionFilter （有异步的AsyncActionFilter）
    /// ResultFilter
    /// 
    /// 顺序 1.AuthorizationFilte
    ///      2.ResourchFilter--OnResourceExecuting方法 (执行后会进入Controler的构造函数内，所以ResourceFilter发生时是在Controller之前)
    ///      3.ActionFilter-OnActionExecuting方法 （ 执行后执行方法中的内容）
    ///      4.ActionFilter-OnActionExecuted方法 
    ///      5.ResultFilter--OnResultExecuting方法
    ///      6.ResultFilter--OnResultExecuted方法 （视图替换环节）
    ///      7.ResourchFilter--OnResourceExecuted方法
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ErrorViewModel model;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ErrorViewModel model)
        {
            _logger = logger;
            this.model = model;
        }

        //[CustomExceptionFilter] 构造时不需要参数时
        //[ServiceFilter(typeof(CustomExceptionFilterAttribute))]  //需要参数时在方法或控制器使用要用ServiceFilter辅助
        //  [TypeFilter(typeof(CustomExceptionFilterAttribute))] // 不用在startUp的ConfiguraServie里面进行注册，使用比较方便

        [CustomFilterFactory(typeof(CustomExceptionFilterAttribute))] //通过继承接口实现，其实ServiceFilter是这种方式的疯封装，
        public IActionResult Index()
        {
            _logger.LogError($"日志");
            int a = 1;
            int b = 0;
            int c = a / b;
       
            return View();
        }

        public IActionResult Index2()
        {
            int a = 1;
            int b = 0;
            int c = a / b;

            return View();
        }

        //方法过滤器
        [CustomActionFiilterAttribute]
        public IActionResult Index3()
        {
            Console.WriteLine("this is Action");
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

        /// <summary>
        /// Core里面过滤器的有5种:
        /// 
        /// AuthorizationFilter
        /// ResourchFilter
        /// ExceptionFilter （报异常是才会执行）
        /// ActionFilter （有异步的AsyncActionFilter）
        /// ResultFilter
        /// 
        /// 顺序 1.AuthorizationFilte
        ///      2.ResourchFilter--OnResourceExecuting方法 (执行后会进入Controler的构造函数内，所以ResourceFilter发生时是在Controller之前)
        ///      3.ActionFilter-OnActionExecuting方法 （ 执行后执行方法中的内容）
        ///      4.ActionFilter-OnActionExecuted方法 
        ///      5.ResultFilter--OnResultExecuting方法
        ///      6.ResultFilter--OnResultExecuted方法 （视图替换环节）
        ///      7.ResourchFilter--OnResourceExecuted方法
        ///      
        /// 通过执行该方法可以看到过滤器的执行顺序
        /// </summary>
        [CustomActionFiilterAttribute]
        [CustomResouseFilterAttribute]
        [CutomResultFilterAttribute]
        public IActionResult TestOrder()
        {
            return View();
        }

        /// <summary>
        /// 可以看出，经过缓存后，方法不会进到Controller，和该方法内
        /// 但是因为缓存的Context.Result，展示的时候前段要执行一下，所以前段视图的时间会变化
        /// </summary>
        /// <returns></returns>
        [CustomCacheResourceFilterAttribute]
        public IActionResult ResouseCache()
        {
            ViewBag.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
            return View();
        }

        /// <summary>
        /// ResponseCache是客户端缓存，其实现原理是在reponse的时候header里面添加了参数告诉客户端浏览器去缓存当前的页面，这样下次
        /// 执行就不会进到方法里面，直接从浏览器拿数据
        /// 
        /// 
        /// 重点！！！！
        ///  ResponseCache也可以做服务器端的缓存，要在StartUp文件里面进行指定
        ///  services.AddResponseCaching();
        ///  app.UseResponseCaching();
        ///  然后ResponseCache特性只是来配合着来实现服务器端缓存，用来打标记
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 60)]
        // [CustomResultFilterAttribute(Duration = 60)]
        public IActionResult ClientCache()
        {
            //ResponseCache 其实是下面一句代码的封装
            //base.HttpContext.Response.Headers["Cache-Control"] = "public,max-age=60";
            ViewBag.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
            return View();
        }

    }
}
