using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //开始命令行模式，这样可以获取命令行启动服务时输入的参数
            var config = new ConfigurationBuilder()
                   .AddCommandLine(args)//支持命令行参数
                  .Build();

            //1.秒杀：
            //string id = config["id"];
            //int minute = Convert.ToInt32(config["minute"]);

            //Seckill.Seckill1(id, minute);  
            //Seckill.Seckill2(id, minute);  

            //2.事务
            //RedisTransaction.TransationDemo();
            //RedisTransaction.TransationDemo2();

            //3.分布式锁
            RedisDistributedLock.Skills(15);

        }

    }
}
