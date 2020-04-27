using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicroService.ClientDemo.Models;
using Wcs.BLL;
using Wcs.Models;
using MicroService.ClientDem.Unility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Consul;

namespace MicroService.ClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentBLL istudentBLL;
        private readonly IConfiguration iconfiguration;
        public HomeController(ILogger<HomeController> logger, IStudentBLL istudentBLL, IConfiguration iconfiguration)
        {
            _logger = logger;
            this.istudentBLL = istudentBLL;
            this.iconfiguration = iconfiguration;
        }
        private static int iSeed = 0;
        public IActionResult Index()
        {
            //List<StudentModel> studentModelList= istudentBLL.GetStudentList();

            #region 通过访问接口进行获取数据 
            //通过访问接口进行获取数据
            //string url = "http://127.0.0.1:6001/api/student/getall";
            //string content = HttpHelper.WebApiHttpGet(url);
            //JObject obj = JsonConvert.DeserializeObject<JObject>(content); 
            //content = JsonConvert.SerializeObject(obj.Value<JArray>("data"));
            //List<StudentModel> studentModelList = JsonConvert.DeserializeObject<List<StudentModel>>(content);
            #endregion

            #region 通过Consul获取接口地址
            //nuget 引入Conful包

            string consulHost = iconfiguration["ConsulHost"];

            ConsulClient client = new ConsulClient(config =>
           {
               config.Address = new Uri(consulHost);
               config.Datacenter = "dc1";
           });

            string groupName = "WcsGroup";
            var agentList = client.Agent.Services().Result.Response
                   .Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();//找到的全部服务;

            AgentService agentService = null;
            /*在选择服务上可以做一些策略*/
            //1.第一个
            //agentService = agentList.First().Value;
            //2.均衡策略
            //agentService = agentList[new Random(iSeed++).Next(0, agentList.Length)].Value;
            //3.轮询策略 
            agentService = agentList[iSeed++ % agentList.Length].Value;

            //4.权重策略
            //客户端必须知道服务实例的权重---注册consul时提供的--1/3/6  ,客户端注册时
            //List<KeyValuePair<string, AgentService>> pairsList = new List<KeyValuePair<string, AgentService>>();
            //foreach (var pair in agentList)
            //{
            //    int count = int.Parse(pair.Value.Tags?[0]);
            //    for (int i = 0; i < count; i++)
            //    {
            //        pairsList.Add(pair);
            //    }
            //}
            //agentService = pairsList[new Random(iSeed++).Next(0, pairsList.Count())].Value;


            //直接访问Consul找到的服务实例进行调用
            string url = $"http://{agentService.Address}:{agentService.Port}/api/student/getall";

            // url = $"{configuration["OcelotGateWayHost"]}/way/home/getstring"; //访问Ocelot的路由
            string content = HttpHelper.WebApiHttpGet(url);
            JObject obj = JsonConvert.DeserializeObject<JObject>(content);
            content = JsonConvert.SerializeObject(obj.Value<JArray>("data"));
            List<StudentModel> studentModelList = JsonConvert.DeserializeObject<List<StudentModel>>(content);
            #endregion

            ViewBag.Url = url;
            return View(studentModelList);
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
