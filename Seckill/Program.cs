using Microsoft.Extensions.Configuration;
using RedisDemo.Demo;
using ServiceStack.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedisDemo
{
    class Program
    {
        /// <summary>
        /// dotnet RedisDemo.dll --id=1 --minute=10
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //开始命令行模式，这样可以获取命令行启动服务时输入的参数
            var config = new ConfigurationBuilder()
                   .AddCommandLine(args)//支持命令行参数
                  .Build();

            //1.秒杀：
            string id = config["id"];
            int minute = Convert.ToInt32(config["minute"]);

            Console.WriteLine("请输入您要运行的方法");
            string inputStr = Console.ReadLine();

            if (inputStr == "Seckill1")
            {
                Seckill.Seckill1(id, minute);
                Seckill.Seckill2(id, minute);
            }
            else if (inputStr == "Transaction")
            {
                //2.事务
                RedisTransaction.TransationDemo();
                RedisTransaction.TransationDemo2();
            }
            else if (inputStr == "DistributedLock")
            {
                //3.分布式锁
                RedisDistributedLock.Skills(minute, 15);
            }
            else if (inputStr == "PublishMsg") //发布消息
            {
                RedisMq.PublishMsg();
            }
            else if (inputStr == "SubscriptionMsg") //订阅消息
            {
                RedisMq.SubscriptionMsg();
            }



        }

    }
}
