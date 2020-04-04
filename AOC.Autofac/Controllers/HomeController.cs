using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AOC.Autofac.Models;
using Wcs.Models.Interface;

namespace AOC.Autofac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _imessageService;
        public HomeController(ILogger<HomeController> logger, IMessageService imessageService)
        {
            _logger = logger;
            _imessageService = imessageService;
        }

        public IActionResult Index()
        {
            _imessageService.Send();
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
