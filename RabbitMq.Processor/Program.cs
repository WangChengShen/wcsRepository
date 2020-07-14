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

                Console.WriteLine($"Hello World! {id}_{timeSpan}_{queueName}");

                //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20
                //生产者消费者模式处理
                //receive1(id, timeSpan);

                //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20 --queue=OrderAll
                //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20 --queue=SMSQueue
                //dotnet RabbitMq.Processor.dll  --id=1 --timespan=20 --queue=EmailQueue
                //发布订阅模式，订阅哪个队列处理哪个队列的消息
                //receive2(id, queueName);

                receive1_rpc(id, timeSpan);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();

        }

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
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine($"消费者{id} 接受消息: {message} {Thread.CurrentThread.ManagedThreadId}");
                                Thread.Sleep(timeSpan);
                            };

                            //注意（之前理解有误）：取消息的时候不绑定交换机，根据队列取就可以了
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
                            };

                            //注意（之前理解有误）：取消息的时候不绑定交换机，根据队列取就可以了
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
                        channel.QueueBind(queue: queueName, exchange: "OrderAllChangeFanout", routingKey: string.Empty, arguments: null);

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
                                             autoAck: true,
                                             consumer: consumer);
                        Console.ReadLine();
                    }
                }
            }
            #endregion
        }
    }
}
