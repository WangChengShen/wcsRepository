using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMq.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region 命令行参数
                var builder = new ConfigurationBuilder()
                   .AddCommandLine(args);

                var configuration = builder.Build();
                string id = configuration["id"];
                int.TryParse(configuration["timespan"] ?? "1", out int timeSpan);
                string queueName = configuration["queue"];
                #endregion

                Console.WriteLine($"准备就绪，Id:{id};timeSpan:{timeSpan};queueName:{queueName}");

                string inputStr = Console.ReadLine();

                if (inputStr == "1")
                {
                    //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20
                    //生产者消费者模式处理：一个消息仅有一个消费者；
                    receive1(id, timeSpan);
                }
                else if (inputStr == "2")
                {
                    // dotnet RabbitMq.Processor.dll--id = 1--timespan = 20--queue = OrderAll
                    //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20 --queue=SMSQueue
                    //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20 --queue=EmailQueue
                    //发布订阅模式，订阅哪个队列处理哪个队列的消息
                    receive2(id, queueName);
                }
                else if (inputStr == "3")
                {
                    receive1_rpc(id, timeSpan);
                }
                else if (inputStr == "log")
                {
                    //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20 --queue=AllLogQueue
                    //dotnet RabbitMq.Processor.dll--id = 1--timespan = 20--queue = ErrorLogQueue
                    receiveLog(queueName);
                }
                else if (inputStr == "secKill")
                {
                    //dotnet RabbitMq.Processor.dll   
                    //dotnet RabbitMq.Processor.dll 
                    secKill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        /// <summary>
        ///生产者消费者模式处理：一个消息仅有一个消费者；
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeSpan"></param>
        static void receive1(string id, int timeSpan)
        {
            #region 生产者消费者
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
                    using (var channel = connection.CreateModel())
                    {
                        try
                        {

                            /*问题：解决消费者创建链接时队列不存在报错的问题
                             * 
                             消费者在处理消息时，如果处理的队列在Mq服务中不存在的话会报错，可以在连接时
                             把队列创建出来，创建后再链接就可以了；
                             发布消息时如果该队列如果存在的话，则直接用该队列
                             
                             */
                            //声明交换机
                            //channel.ExchangeDeclare(exchange: "OrderOnlyChange",
                            //       type: ExchangeType.Direct,
                            //       durable: true,
                            //       autoDelete: false,
                            //       arguments: null);

                            ////声明队列
                            //channel.QueueDeclare(queue: "OrderOnly",
                            //   durable: true,
                            //   exclusive: false,
                            //   autoDelete: false,
                            //   arguments:null);

                            ////队列绑定到交换机
                            //channel.QueueBind(queue: "OrderOnly",
                            //             exchange: "OrderOnlyChange",
                            //             routingKey: "OrderOnlyKey", arguments: null);

                            //Console.BackgroundColor= ConsoleColor.Yellow;
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"消费者{id} 接受消息: {message} {Thread.CurrentThread.ManagedThreadId}，优先级：{ea.BasicProperties.Priority}");
                                Thread.Sleep(timeSpan);
                            };

                            //注意（之前理解有误）：取消息的时候不绑定交换机，根据队列取就可以了
                            //队列绑定到交换机
                            //channel.QueueBind(queue: "OrderOnly",
                            //             exchange: "OrderOnlyChange",
                            //             routingKey: string.Empty, arguments: null);

                            channel.BasicConsume(queue: "OrderOnly",
                                         autoAck: true,
                                         consumer: consumer);
                            Console.WriteLine(" Press [enter] to exit.");
                            Console.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 生产者消费者模式处理：一个消息仅有一个消费者；
        /// 且带有回复消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeSpan"></param>
        static void receive1_rpc(string id, int timeSpan)
        {
            #region 生产者消费者
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
                    using (var channel = connection.CreateModel())
                    {
                        try
                        {
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                //接受消息
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"消费者{id} 接受消息: {message},corrId={ea.BasicProperties.CorrelationId}"); //{Thread.CurrentThread.ManagedThreadId}
                                Thread.Sleep(timeSpan);

                                //回复消息，把回复放入到回复队列中
                                IBasicProperties props = ea.BasicProperties;
                                IBasicProperties replyProps = channel.CreateBasicProperties();
                                replyProps.CorrelationId = props.CorrelationId;

                                string resMsg = $"消息（{replyProps.CorrelationId}）:" + new Random().Next(10);
                                var responseBytes = Encoding.UTF8.GetBytes(resMsg);
                                channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);//注意，回复是交换机名称为空，代表默认的交换机
                                Console.WriteLine($"回复消息：{resMsg},ReplyTo={props.ReplyTo}");


                                /*手动应答的方式，处理过之后把消息去掉；现在用自动应答收到后把消息从队列中删除
                                 * consumer.Model.BasicAck(eargs.DeliveryTag, false); 
                                 手动应答时，处理失败时，把消息重新放回到队列
                                  channel.BasicReject(eargs.DeliveryTag, true);
                                */
                            };

                            //注意（之前理解有误）：取消息的时候不绑定交换机，根据队列取就可以了
                            channel.BasicConsume(queue: "OrderOnly2",
                                         autoAck: true,//是否自动应答
                                         consumer: consumer);
                            Console.WriteLine(" Press [enter] to exit.");
                            Console.ReadLine();
                            /*
                             控制台里面后面要加一个Console.ReadLine();让程序阻塞，以至于mq的conn不关闭；
                             但是在windows服务里面，如果在OnStart里面进行消费消息，要把conn和信道channel放到外面，这样
                             OnStart执行之后，链接和信道才不会关闭，可以实践阻塞进行消费消息
                             */
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 发布订阅模式，订阅某个队列就消费该队列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queueName"></param>
        static void receive2(string id, string queueName)
        {
            #region 发布订阅
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
                    using (var channel = connection.CreateModel())
                    {
                        //声明交换机exchang
                        channel.ExchangeDeclare(exchange: "OrderAllChangeFanout",
                                                type: ExchangeType.Fanout,
                                                durable: true,
                                                autoDelete: false,
                                                arguments: null);
                        //声明队列SMSqueue
                        channel.QueueDeclare(queue: queueName,
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
                        //绑定
                        channel.QueueBind(queue: queueName,
                            exchange: "OrderAllChangeFanout",
                            routingKey: string.Empty,
                            arguments: null);

                        //定义消费者                                      
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine($"订阅{id}接收到消息：{message},完成{queueName}处理");
                        };
                        Console.WriteLine($"订阅{id}已经准备 {queueName} 就绪...");
                        //处理消息
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,//自动应答
                                             consumer: consumer);

                        /*用于阻塞程序，若不是控制台程序（如winService），不要把connection 和 channel 销废就可以了*/
                        Console.ReadLine();
                    }
                }
            }
            #endregion
        }

        /// <summary>
        ///处理日志的消费者
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeSpan"></param>
        static void receiveLog(string queue)
        {
            #region 生产者消费者
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
                    using (var channel = connection.CreateModel())
                    {
                        try
                        {

                            /*问题：解决消费者创建链接时队列不存在报错的问题
                             * 
                             消费者在处理消息时，如果处理的队列在Mq服务中不存在的话会报错，可以在连接时
                             把队列创建出来，创建后再链接就可以了；
                             发布消息时如果该队列如果存在的话，则直接用该队列 
                             */
                            //声明交换机
                            channel.ExchangeDeclare(exchange: "LogExchange",
                                   type: ExchangeType.Direct,
                                   durable: true,
                                   autoDelete: false,
                                   arguments: null);

                            channel.QueueDeclare("AllLogQueue", durable: true, exclusive: false,
                           autoDelete: false, arguments: null);

                            channel.QueueDeclare("ErrorLogQueue", durable: true, exclusive: false,
                               autoDelete: false, arguments: null);

                            string[] errorLevel = new string[] { "info", "warn", "debug", "error" };
                            foreach (var item in errorLevel)
                            {
                                //AllLogQueue,绑定所有的log等级routeKey
                                channel.QueueBind(queue: "AllLogQueue", exchange: "LogExchange", routingKey: item, arguments: null);
                            }

                            channel.QueueBind(queue: "ErrorLogQueue", exchange: "LogExchange", routingKey: "error", arguments: null);

                            //Console.BackgroundColor= ConsoleColor.Yellow;
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($" 接受消息: {message} ");
                            };

                            channel.BasicConsume(queue: queue,
                                         autoAck: true,
                                         consumer: consumer);

                            Console.WriteLine(" Press [enter] to exit.");
                            Console.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 秒杀
        /// </summary>
        static void secKill()
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
                using (var channel = connection.CreateModel())
                {
                    try
                    {

                        /*问题：解决消费者创建链接时队列不存在报错的问题
                         * 
                         消费者在处理消息时，如果处理的队列在Mq服务中不存在的话会报错，可以在连接时
                         把队列创建出来，创建后再链接就可以了；
                         发布消息时如果该队列如果存在的话，则直接用该队列 
                         */
                        //声明交换机
                        //声明队列
                        channel.QueueDeclare(queue: "SecKillQueue",
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

                        //声明交换机
                        channel.ExchangeDeclare(exchange: "SecKillChange",
                               type: ExchangeType.Direct,
                               durable: true,
                               autoDelete: false,
                               arguments: null);

                        //队列绑定到交换机
                        channel.QueueBind(queue: "SecKillQueue",
                                     exchange: "SecKillChange",
                                     routingKey: "SecKillKey", arguments: null);


                        //Console.BackgroundColor= ConsoleColor.Yellow;
                        var consumer = new EventingBasicConsumer(channel);
                        int num = 0;
                        consumer.Received += (model, ea) =>
                        {
                            num++;
                            if (num <= 10)
                            {
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($" {message},并成功秒杀成功 ");
                            }
                        };

                        channel.BasicConsume(queue: "SecKillQueue",
                                     autoAck: true,
                                     consumer: consumer);

                        Console.WriteLine("准备就绪.");
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
