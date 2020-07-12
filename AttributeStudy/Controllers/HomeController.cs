using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AttributeStudy.Models;
using Microsoft.Extensions.Options;

namespace AttributeStudy.Controllers
{
    public class HomeController : Controller
    {
        //log的使用注入ILogger<HomeController>或ILoggerFactory
        private readonly ILogger<HomeController> _logger;
        private readonly ILoggerFactory loggerFactory;

        private readonly IOptions<AppSetting> _appSetting;
        private readonly IOptions<Logging> _logging;
        public HomeController(ILogger<HomeController> logger, IOptions<AppSetting> appSetting, IOptions<Logging> logging, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _appSetting = appSetting;
            _logging = logging;
            this.loggerFactory = loggerFactory;
        }

        public IActionResult Index()
        {
            _logger.LogWarning("this is HomeController Index");
            loggerFactory.CreateLogger<HomeController>().LogWarning("this is HomeController Index2");

            ViewBag.LogDefault = _appSetting.Value.Logging.LogLevel.Default;
            ViewBag.Microsoft = _logging.Value.LogLevel.Microsoft;
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

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            string errorMsg = string.Empty;
            if (!ModelState.IsValid)
            {
                var firstErrorFiled = ModelState.Where(m => m.Value.Errors.Count > 0).First();
                errorMsg = firstErrorFiled.Value.Errors.First().ErrorMessage;
                return Json(new { suc = 0, msg = errorMsg });
            }
            return Json(new { suc = 1, msg = "123" });
        }

    }
}
