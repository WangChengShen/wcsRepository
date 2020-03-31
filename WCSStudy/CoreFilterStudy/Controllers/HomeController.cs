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
    ///      2.ResourchFilter--OnResourceExecuting方法
    ///      3.ActionFilter-OnActionExecuting方法 （ 执行后执行方法中的内容）
    ///      4.ActionFilter-OnActionExecuted方法 
    ///      5.ResultFilter--OnResultExecuting方法
    ///      6.ResultFilter--OnResultExecuted方法 （视图替换环节）
    ///      7.ResourchFilter--OnResourceExecuted方法
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[CustomExceptionFilter] 构造时不需要参数时
        //[ServiceFilter(typeof(CustomExceptionFilterAttribute))]  //需要参数时在方法或控制器使用要用ServiceFilter辅助
        //  [TypeFilter(typeof(CustomExceptionFilterAttribute))] // 不用在startUp的ConfiguraServie里面进行注册，使用比较方便

        [CustomFilterFactory(typeof(CustomExceptionFilterAttribute))] //通过继承接口实现，其实ServiceFilter是这种方式的疯封装，
        public IActionResult Index()
        {
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

        [CustomActionFiilterAttribute]
        [CustomResouseFilterAttribute] 
        [CutomResultFilterAttribute]
        public IActionResult TestOrder()
        {
            return View();
        }


    }
}
