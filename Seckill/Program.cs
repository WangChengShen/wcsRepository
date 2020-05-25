using Microsoft.Extensions.Configuration;
using Seckill.Redis;
using ServiceStack.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Seckill
{
    class Program
    {
        static void Main(string[] args)
        {
            //开始命令行模式，这样可以获取命令行启动服务时输入的参数
            var config = new ConfigurationBuilder()
                   .AddCommandLine(args)//支持命令行参数
                  .Build();
             
            string id = config["id"];
            int minute = Convert.ToInt32(config["minute"]);
            // seckill(id, minute);
            seckill2(id, minute);

            //Console.WriteLine(RedisHelper.GetStringValue("wcs"));
        }

        /// <summary>
        /// StackExchange.Redis 实现秒杀
        /// </summary>
        /// <param name="id"></param>
        /// <param name="minute"></param>
        static void seckill(string id, int minute)
        {
             RedisHelper.SetStringValue("stock", "10");
            Console.WriteLine($"在{minute}分0秒开始抢购");

            var start = true;
            while (start)
            {
                if (minute == DateTime.Now.Minute)
                {
                    start = false;

                    for (int i = 0; i < 10; i++)
                    {
                        string name = $"客户端{id}:{i}";
                        Task.Run(() =>
                        {
                            long result = RedisHelper.StringDecrement("stock");
                            if (result >= 0)
                            {
                                Console.WriteLine($"{name}******************抢购成功**************");
                            }
                            else
                            {
                                Console.WriteLine($"{name}抢购失败");
                            }
                        });
                    }
                }
            }
        }

        /// <summary>
        /// ServiceStack.Redis（收费的） 实现秒杀，
        /// </summary>
        /// <param name="id"></param>
        /// <param name="minute"></param>
        static void seckill2(string id, int minute)
        {
            using (RedisClient redisClient = new RedisClient("127.0.0.1", 6379))
            {
                //ServiceStack的例子，stock在存的时候要用int值，不然会报错
                redisClient.Set<int>("stock", 10); 
            }
            Console.WriteLine($"在{minute}分0秒开始抢购");

            var start = true;
            while (start)
            {
                if (minute == DateTime.Now.Minute)
                {
                    start = false;

                    for (int i = 0; i < 10; i++)
                    {
                        string name = $"客户端{id}:{i}"; 
                        Task.Run(() =>
                        {  
                            using (RedisClient redisClient = new RedisClient("127.0.0.1", 6379))
                            { 
                                long result = redisClient.Decr("stock"); 
                                if (result >= 0)
                                {
                                    Console.WriteLine($"{name}******************抢购成功**************");
                                }
                                else
                                {
                                    Console.WriteLine($"{name}抢购失败");
                                }
                            }

                        });
                        Thread.Sleep(10);
                    }
                }
            }
        }
    }
}
