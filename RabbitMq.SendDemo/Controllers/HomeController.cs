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


                    /*重要！！！
                     * 绑定到交换机时可以绑多个routingKey，这样一个队列就会有多个的routeKey，
                     * 发消息是要匹配到任意一个key就会发送到该队列*/
                    //如下：给OrderOnly2队列有绑了一个OrderOnlyKey3的key
                    //channel.QueueBind(queue: "OrderOnly2",
                    //      exchange: "OrderOnlyChange",
                    //      routingKey: "OrderOnlyKey3", arguments: null);

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


        /// <summary>
        /// RabbitMq 消息优先级的使用
        /// 优先级别高的消息被优先处理：条件是要把一批消息都发布进入队列，然后再处理，这样优先级高的会先被处理；
        /// 如果随时发随时消费，则还是按照顺序来处理；
        /// </summary>
        /// <returns></returns>
        public IActionResult InserPriority()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "192.168.1.69";//RabbitMQ服务在本地运行
            factory.UserName = "gkbpo";//用户名
            factory.Password = "gkbpo123";//密码
            factory.VirtualHost = "newbpo";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //声明交换机
                    channel.ExchangeDeclare(exchange: "OrderOnlyChange",
                           type: ExchangeType.Direct,
                           durable: true,
                           autoDelete: false,
                           arguments: null);

                    //声明队列
                    channel.QueueDeclare(queue: "OrderOnly",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: new Dictionary<string, object> {
                           { "x-max-priority", 10 }//指定队列支持优先级设置
                       });

                    //队列绑定到交换机
                    channel.QueueBind(queue: "OrderOnly",
                                 exchange: "OrderOnlyChange",
                                 routingKey: "OrderOnlyKey", arguments: null);


                    string[] msgArray = new string[] {
                        "QQ VIP1:我……",
                        "张三：abc",
                        "李四：abc",
                        "王二：abc",
                        "QQ VIP2:我……"
                    };
                    IBasicProperties props = channel.CreateBasicProperties();

                    foreach (var msg in msgArray)
                    {
                        if (msg.Contains("VIP"))
                        {
                            props.Priority = 9; //级别越高，越先被处理
                        }
                        else
                        {
                            props.Priority = 1;
                        }
                        string message = msg;
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OrderOnlyChange",
                                        routingKey: "OrderOnlyKey",
                                        basicProperties: props,
                                        body: body);
                        Console.WriteLine($"消息：{message} 已发送~");
                    }


                }
            }

            return Content("Success");
        }

        /// <summary>
        /// 场景：把info、warn、debug、error四种等级的日志记录下来；另外error等级的日志要发邮件给开发人员；
        /// 解决方案：准备两个队列，一个方info、warn、debug、error四种的消息;另一个队列再放error的消息；
        /// 并把消息持久化下来
        /// </summary>
        /// <returns></returns>
        public IActionResult LogMsg()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "192.168.1.69";//RabbitMQ服务在本地运行
            factory.UserName = "gkbpo";//用户名
            factory.Password = "gkbpo123";//密码
            factory.VirtualHost = "newbpo";

            using (IConnection conn = factory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    /*
                     持久化(服务宕机后也不会丢失)：
                     1、交换机要设置为持久化
                     2、队列要设置为持久化
                     3、消息发送时持久化
                    */
                    //声明交换机
                    channel.ExchangeDeclare(exchange: "LogExchange",
                           type: ExchangeType.Direct,
                           durable: true,//1.交换机要设置为持久化
                           autoDelete: false,
                           arguments: null);

                    // 2、队列要设置为持久化
                    channel.QueueDeclare("AllLogQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueDeclare("ErrorLogQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    string[] errorLevel = new string[] { "info", "warn", "debug", "error" };
                    foreach (var item in errorLevel)
                    {
                        //AllLogQueue,绑定所有的log等级routeKey
                        channel.QueueBind(queue: "AllLogQueue", exchange: "LogExchange", routingKey: item, arguments: null);
                    }

                    channel.QueueBind(queue: "ErrorLogQueue", exchange: "LogExchange", routingKey: "error", arguments: null);

                    List<MsgModel> msgList = new List<MsgModel>();

                    for (int i = 0; i < 100; i++)
                    {
                        if (i % 4 == 0)
                        {
                            msgList.Add(new MsgModel
                            {
                                LogType = "info",
                                LogMsg = $"info 日志{msgList.Count(m => m.LogType == "info") + 1}：……"
                            });
                        }
                        if (i % 4 == 1)
                        {
                            msgList.Add(new MsgModel
                            {
                                LogType = "warn",
                                LogMsg = $"warn 日志{msgList.Count(m => m.LogType == "warn") + 1}：……"
                            });
                        }
                        if (i % 4 == 2)
                        {
                            msgList.Add(new MsgModel
                            {
                                LogType = "debug",
                                LogMsg = $"debug 日志{msgList.Count(m => m.LogType == "debug") + 1}：……"
                            });
                        }
                        if (i % 4 == 3)
                        {
                            msgList.Add(new MsgModel
                            {
                                LogType = "error",
                                LogMsg = $"error 日志{msgList.Count(m => m.LogType == "error") + 1}：……"
                            });
                        }
                    }
                    IBasicProperties properties = channel.CreateBasicProperties();
                    foreach (var item in msgList)
                    {
                        byte[] body = Encoding.UTF8.GetBytes(item.LogMsg);

                        properties.Persistent = true; //3.消息发送时设置持久化

                        channel.BasicPublish(exchange: "LogExchange",
                                           routingKey: item.LogType,
                                           basicProperties: properties, //加上basicProperties参数，绑定回复队列
                                           body: body);

                        Console.WriteLine($"消息：{item.LogMsg} 已发送~");
                    }

                    return Content("成功发送");
                }
            }
        }

        /// <summary>
        /// 1.exchange有4种：Direct,Fanout,Topic,Header
        /// 2.Direct(根据路由键精准匹配),Fanout(忽略路由键，广播模式),
        ///   Topic(根据路由键模糊匹配，*匹配一个单词，#匹配多个单词)；
        /// 3.Header根据发送的消息内容的Header属性进行匹配，在绑定Queue与exchange时指定一组键值对数据以及x-match
        ///   参数，字符串类型，可以设置为all或者any，all代表所有数据都匹配成功才会匹配成功，any是匹配上任一组数据
        ///   即匹配成功
        /// </summary>
        /// <returns></returns>
        public IActionResult HeaderExchange()
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
                    //声明交换机
                    channel.ExchangeDeclare(exchange: "HeaderExchange",
                           type: ExchangeType.Headers,
                           durable: true,
                           autoDelete: false,
                           arguments: null);

                    //声明队列
                    channel.QueueDeclare(queue: "HeaderAllQueue",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

                    //声明队列
                    channel.QueueDeclare(queue: "HeaderAnyQueue",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

                    //队列绑定到交换机
                    channel.QueueBind(queue: "HeaderAllQueue", exchange: "HeaderExchange", routingKey: "",
                    arguments: new Dictionary<string, object>
                    {
                        //x-match是要有的，值可设置为all或any
                        { "x-match","all"}, 
                        //下面的参数是业务参数
                        { "chinese","90"},
                        { "english","80"},
                    });


                    //队列绑定到交换机
                    channel.QueueBind(queue: "HeaderAnyQueue", exchange: "HeaderExchange", routingKey: "",
                    arguments: new Dictionary<string, object>
                    {
                        //x-match是要有的，值可设置为all或any
                        { "x-match","any"}, 
                        //下面的参数是业务参数
                        { "chinese","90"},
                        { "english","80"},
                    });

                    {
                        var props = channel.CreateBasicProperties();
                        props.Headers = new Dictionary<string, object>() {
                                                                              { "chinese","90"},
                                                                              { "english","80"},
                                                                          };

                        /*chinese和english都相同；chinese和english也符合任一个相同；
                         * 消息会同时发送到HeaderAnyQueue和HeaderAllQueue*/
                        string msg = @"chinese=90;english=80";

                        var body = Encoding.UTF8.GetBytes(msg);
                        //基本发布
                        channel.BasicPublish(exchange: "HeaderExchange",
                                             routingKey: string.Empty,
                                             basicProperties: props,
                                             body: body);

                        Console.WriteLine($"{msg}已发送");
                    }

                    {
                        var props = channel.CreateBasicProperties();
                        props.Headers = new Dictionary<string, object>() {
                                                                              { "chinese","90"},
                                                                              { "english","100"},
                                                                          };
                        /*chinese和english有一个不相同；chinese和english符合任一个相同；
                         * 消息会发送到HeaderAnyQueue,不会发送到HeaderAllQueue
                         */
                        string msg = @"chinese=90；english=100；";

                        var body = Encoding.UTF8.GetBytes(msg);
                        //基本发布
                        channel.BasicPublish(exchange: "HeaderExchange",
                                             routingKey: string.Empty,
                                             basicProperties: props,
                                             body: body);

                        Console.WriteLine($"{msg}已发送");
                    }

                    return Content("已发送");
                }
            }
        }

    }
}
