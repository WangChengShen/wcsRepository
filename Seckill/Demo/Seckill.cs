using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisDemo
{
    public class Seckill
    {

        /// <summary>
        /// StackExchange.Redis 实现秒杀
        /// </summary>
        /// <param name="id"></param>
        /// <param name="minute"></param>
        public static void Seckill1(string id, int minute)
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
        public static void Seckill2(string id, int minute)
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
