using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AOC.Autofac.Models;
using Wcs.DAL;
using Wcs.BLL;
using Newtonsoft.Json;

namespace AOC.Autofac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageDAL _imessageService;
        private readonly IStudentBLL studentBLL;

        public IStudentBLL studentBLL2 { get; set; }
        public HomeController(ILogger<HomeController> logger, IMessageDAL imessageService, IStudentBLL studentBLL)
        {
            _logger = logger;
            _imessageService = imessageService;
            this.studentBLL = studentBLL;
        }

        public IActionResult Index()
        {
            _imessageService.Send();
            Console.WriteLine(JsonConvert.SerializeObject(studentBLL.GetStudentList()));

            string msg = "属性注入：" + JsonConvert.SerializeObject(studentBLL2.GetStudentList());
            Console.WriteLine(msg);
            ViewBag.Msg = msg;
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
