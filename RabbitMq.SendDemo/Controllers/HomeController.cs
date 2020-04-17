using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMq.SendDemo.Models;
using RabbitMQ.Client;

namespace RabbitMq.SendDemo.Controllers
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
        /// dotnet RabbitMq.SendDemo.dll  --urls="http://*:6001" --ip="127.0.0.1" --port=6001
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Index(int? id)
        {
            this.Insert(id ?? 5);
            return View();
        }

        public IActionResult IndexAll(int? id)
        {
            this.InsertAll(id ?? 5);
            return Content("成功发送");
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
        /// //发消息对队列,生产者消费者模式：一个消息仅有一个消费者；
        /// </summary>
        /// <param name="count"></param>
        private void Insert(int count)
        {
            #region ConnectionFactory
            var factory = new ConnectionFactory();
            factory.HostName = "192.168.1.69";//RabbitMQ服务在本地运行
            factory.UserName = "gkbpo";//用户名
            factory.Password = "gkbpo123";//密码
            factory.VirtualHost = "newbpo";
            #endregion

            using (var conn = factory.CreateConnection())//创建连接
            {
                using (IModel channel = conn.CreateModel()) //创建对象
                {
                    //声明队列
                    channel.QueueDeclare(queue: "OrderOnly",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

                    //声明交换机
                    channel.ExchangeDeclare(exchange: "OrderOnlyChange",
                           type: ExchangeType.Direct,
                           durable: true,
                           autoDelete: false,
                           arguments: null);

                    //队列绑定到交换机
                    channel.QueueBind(queue: "OrderOnly",
                                 exchange: "OrderOnlyChange",
                                 routingKey: string.Empty, arguments: null);


                    Console.WriteLine("准备就绪,开始写入~~~");
                    for (int i = 0; i < count; i++)
                    {
                        string message = $"Task{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OrderOnlyChange",
                                        routingKey: string.Empty,
                                        basicProperties: null,
                                        body: body);
                        Console.WriteLine($"消息：{message} 已发送~");
                    }

                }
            }
        }

        /// <summary>
        /// 发消息对多个队列，广播模式
        /// 发布订阅模式
        /// </summary>
        /// <param name="number"></param>
        private void InsertAll(int number)
        { 
            #region ConnectionFactory
            var factory = new ConnectionFactory();
            factory.HostName = "192.168.1.69";//RabbitMQ服务在本地运行
            factory.UserName = "gkbpo";//用户名
            factory.Password = "gkbpo123";//密码
            factory.VirtualHost = "newbpo";
            #endregion

            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //声明交换机，
                    channel.ExchangeDeclare(exchange: "OrderAllChangeFanout",
                          type: ExchangeType.Fanout,
                          durable: true,
                          autoDelete: false,
                          arguments: null);

                    //声明队列，把多个队列绑定到交换机上
                    channel.QueueDeclare(queue: "OrderAll",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

                    channel.QueueBind(queue: "OrderAll",
                                exchange: "OrderAllChangeFanout",
                                routingKey: string.Empty,
                                arguments: null);

                    channel.QueueDeclare(queue: "SMSQueue",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                    channel.QueueBind(queue: "SMSQueue",
                                exchange: "OrderAllChangeFanout",
                                routingKey: string.Empty,
                                arguments: null);

                    channel.QueueDeclare(queue: "EmailQueue",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                    channel.QueueBind(queue: "EmailQueue",
                              exchange: "OrderAllChangeFanout",
                              routingKey: string.Empty,
                              arguments: null);

                    Console.WriteLine("准备就绪,开始写入");
                    for (int i = 0; i < number; i++)
                    {
                        string message = $"来自{this.configuration["port"]}的Task{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OrderAllChangeFanout",
                                        routingKey: string.Empty,
                                        basicProperties: null,
                                        body: body);
                        Console.WriteLine($"消息：{message} 已发送");
                    }
                }
            }
        }

    }
}
