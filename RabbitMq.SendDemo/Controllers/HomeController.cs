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
using RabbitMQ.Client.Events;

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
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// dotnet RabbitMq.SendDemo.dll  --urls="http://*:6001" --ip="127.0.0.1" --port=6001
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult IndexOne(int? id)
        {
            this.Insert(id ?? 5);
            return Content("成功发送");
        }

        /// <summary>
        /// dotnet RabbitMq.SendDemo.dll  --urls="http://*:6001" --ip="127.0.0.1" --port=6001
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult IndexRpc(int? id)
        {
            this.InsertAndGetResponseMsg(id ?? 5);
            return Content("成功发送");
        }

        /// <summary>
        /// 通过事件驱动来获取回复消息
        /// http://127.0.0.1:6001/home/IndexRpcByEvent?id=1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult IndexRpcByEvent(int? id)
        {
            this.InsertAndGetResponseMsgByEnevt(id ?? 5);
            return Content("成功发送");
        }


        public IActionResult IndexAll(int? id)
        {
            this.InsertAll(id ?? 5);
            return Content("成功发送");
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

                    //声明队列2
                    //channel.QueueDeclare(queue: "OrderOnly2",
                    //   durable: true,
                    //   exclusive: false,
                    //   autoDelete: false,
                    //   arguments: null);
                     
                    //声明交换机
                    channel.ExchangeDeclare(exchange: "OrderOnlyChange",
                           type: ExchangeType.Direct,
                           durable: true,
                           autoDelete: false,
                           arguments: null);
                     
                    //队列绑定到交换机
                    channel.QueueBind(queue: "OrderOnly",
                                 exchange: "OrderOnlyChange",
                                 routingKey: "OrderOnlyKey", arguments: null);

                    //队列绑定到交换机
                    //channel.QueueBind(queue: "OrderOnly",
                    //             exchange: "OrderOnlyChange",
                    //             routingKey: "OrderOnlyKey2", arguments: null);

                    /*交换机四种模式：Direct(根据路由键匹配),Fanout（广播）、Topic（根据路由键模糊匹配）、headers
                     * 
                        交换机设置为Direct模式：
                        发消息BasicPublish时可以指定消息发送到哪个路由键上，
                        然后会根据路由键自动找队列，把消息发送到对应的队列里面
                        例：队列OrderOnly2没有绑定路由键OrderOnlyKey，所以消息不会发到这个队列里面
                    */

                    Console.WriteLine("准备就绪,开始写入~~~");
                    for (int i = 0; i < count; i++)
                    {
                        string message = $"Task{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OrderOnlyChange",
                                        routingKey: "OrderOnlyKey",
                                        basicProperties: null,
                                        body: body);
                        Console.WriteLine($"消息：{message} 已发送~");
                    }

                }
            }
        }

        /// <summary>
        /// //发消息对队列,生产者消费者模式：一个消息仅有一个消费者（不推荐此方式）
        /// </summary>
        /// <param name="count"></param>
        private void InsertAndGetResponseMsg(int count)
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

                    #region 设置Rpc回复队列 
                    string replyQueueName = "ReplyQueue";
                    //声明接受消息回复队列
                    channel.QueueDeclare(queue: replyQueueName, durable: true, exclusive: false, autoDelete: false, null);
                    //注意：回复的队列不能绑定到交换机，因为如果绑定了，发送的时候交换机会把消息也发送到这个队列，这样消息会造成混乱
                    //channel.QueueBind(queue: replyQueueName,
                    //             exchange: "OrderOnlyChange",
                    //             routingKey: string.Empty, arguments: null);

                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume(replyQueueName, true, consumer);

                    #endregion
                    for (int i = 0; i < count; i++)
                    {
                        string message = $"Task{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);


                        var corrId = Guid.NewGuid().ToString();
                        var props = channel.CreateBasicProperties();
                        props.ReplyTo = replyQueueName;//你处理完过后把结果返回到这个队列中。
                        props.CorrelationId = corrId;//这是我的请求标识

                        //channel.BasicPublish(exchange: "OrderOnlyChange",
                        //                routingKey: string.Empty,
                        //                basicProperties: null,
                        //                body: body);

                        channel.BasicPublish(exchange: "OrderOnlyChange",
                                        routingKey: string.Empty,
                                        basicProperties: props, //加上basicProperties参数，绑定回复队列
                                        body: body);

                        Console.WriteLine($"消息：{message} 已发送，corrId={corrId}");

                        #region 定义消费者接受消息 
                        //时间驱动方式不行
                        //var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                        //consumer.Received += (model, ea) =>
                        //{
                        //    var body = ea.Body;
                        //    var message = Encoding.UTF8.GetString(body);
                        //    Console.WriteLine($"接收到回复消息：{message},corrId={ea.BasicProperties.CorrelationId }");
                        //}; 
                        ////处理消息
                        //channel.BasicConsume(queue: replyQueueName,
                        //                     autoAck: true,
                        //                     consumer: consumer);

                        //循环机制根据CorrelationId
                        while (true)
                        {
                            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            if (ea.BasicProperties.CorrelationId == corrId)
                            {
                                string replyMsg = Encoding.UTF8.GetString(ea.Body);
                                Console.WriteLine($"收到回复消息：{replyMsg} ");
                                break;
                            }
                        }
                        #endregion
                    }

                }
            }
        }

        /// <summary>
        /// //发消息对队列，通过Rpc事件驱动获取回复消息（推荐此方式）
        /// 
        /// </summary>
        /// <param name="count"></param>
        private void InsertAndGetResponseMsgByEnevt(int count)
        {
            #region ConnectionFactory
            var factory = new ConnectionFactory();
            factory.HostName = "192.168.1.69";//RabbitMQ服务在本地运行
            factory.UserName = "gkbpo";//用户名
            factory.Password = "gkbpo123";//密码
            factory.VirtualHost = "newbpo";
            #endregion
            /*
             * 发布消息，消息找队列，常用3种路由方式：
             Direct：根据路由键进行，消息发送到该交换机且绑定有该路由键的队列；
             Fanout：广播模式，忽略路由键，消息会发送到绑定在交换机上面的所有队列；
             Topic：类似Direct，但是匹配路由键可以根据匹配符进行匹配队列；
             */
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

                    channel.QueueDeclare(queue: "OrderOnly2",
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

                    //队列绑定到交换机，路由Key:OrderOnlyKey
                    channel.QueueBind(queue: "OrderOnly",
                                 exchange: "OrderOnlyChange",
                                 routingKey: "OrderOnlyKey", arguments: null);

                    //绑定到交换机，路由Key:OrderOnlyKey2
                    channel.QueueBind(queue: "OrderOnly2",
                              exchange: "OrderOnlyChange",
                              routingKey: "OrderOnlyKey2", arguments: null);

                    Console.WriteLine("准备就绪,开始写入~~~");

                    #region 设置Rpc回复队列,定义消费者接受消息
                    string replyQueueName = "ReplyQueue";
                    //声明接受消息回复队列
                    channel.QueueDeclare(queue: replyQueueName, durable: true, exclusive: false, autoDelete: false, null);

                    //注意：回复的队列不能绑定到交换机，因为如果绑定了，发送的时候交换机会把消息也发送到这个队列，这样消息会造成混乱
                    //channel.QueueBind(queue: replyQueueName,
                    //             exchange: "OrderOnlyChange",
                    //             routingKey: string.Empty, arguments: null);

                    Dictionary<string, TaskCompletionSource<string>> resultDic = new Dictionary<string, TaskCompletionSource<string>>();
                    var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        //每一次请求的唯一标识
                        string corrId = ea.BasicProperties.CorrelationId;

                        if (resultDic.Keys.Contains(corrId))//如果包含这个corrId,则把结果赋值给对应的TaskCompletionSource
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);

                            //把结果放入到结果集合中
                            resultDic[corrId].SetResult(message);
                            //Console.WriteLine($"接收到回复消息：{message}");
                        }
                    };

                    //处理消息
                    channel.BasicConsume(queue: replyQueueName,
                                         autoAck: true,
                                         consumer: consumer);
                    #endregion

                    for (int i = 0; i < count; i++)
                    {
                        string message = $"Task{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        //设置回复队列参数
                        var corrId = Guid.NewGuid().ToString();
                        var props = channel.CreateBasicProperties();
                        props.ReplyTo = replyQueueName;//你处理完过后把结果返回到这个队列中。
                        props.CorrelationId = corrId;//这是我的请求标识

                        //channel.BasicPublish(exchange: "OrderOnlyChange",
                        //                routingKey: string.Empty,
                        //                basicProperties: null,
                        //                body: body);

                        //利用TaskCompletionSource的特性，异步获取消息时会等待
                        var result = new TaskCompletionSource<string>();
                        resultDic.Add(corrId, result);

                        //因为发送时指定路由key为OrderOnlyKey2，所以消息只会推送到OrderOnly2队列
                        channel.BasicPublish(exchange: "OrderOnlyChange",
                                        routingKey: "OrderOnlyKey2",
                                        basicProperties: props, //加上basicProperties参数，绑定回复队列
                                        body: body);

                        Console.WriteLine($"消息：{message} 已发送，corrId={corrId}");

                        Console.WriteLine($"接收到回复消息：{result.Task.Result}");
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

                    //绑定队列
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
