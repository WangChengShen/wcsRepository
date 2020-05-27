using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisDemo
{
    public class RedisTransaction
    {
        /// <summary>
        /// ServiceStack.Redis
        /// Redis 有事务，但是没有回滚的操作；如果在事务过程中，有地方改了监控的key，则整个事务会提交失败；
        /// </summary>
        public static void TransationDemo()
        {
            bool flag = false;
            using (RedisClient redisClient = new RedisClient("127.0.0.1", 6379))
            {
                redisClient.Set("a", "1");
                redisClient.Set("b", "1");
                redisClient.Set("c", "1");

                redisClient.Watch("a", "b", "c");//要把这些key放入到监控中
                using (var trans = redisClient.CreateTransaction())
                {
                    trans.QueueCommand(p => p.Set("a", "3"));//在这可以加个断点，然后用工具去改a的值为2
                    trans.QueueCommand(p => p.Set("b", "3"));
                    trans.QueueCommand(p => p.Set("c", "3"));

                    flag = trans.Commit();
                }
                Console.WriteLine($"提交结果：{flag}，获取值a:{redisClient.Get<string>("a")},b:{redisClient.Get<string>("b")},c:{redisClient.Get<string>("c")}");
            }
        }

        /// <summary>
        /// StackExchange.Redis 实现事务
        /// </summary>
        public static void TransationDemo2()
        {
            bool flag = false;

            IDatabase database = RedisHelper.db;
            database.StringSet("a", "11");
            database.StringSet("b", "11");
            database.StringSet("c", "11");

            var tran = database.CreateTransaction();
            tran.AddCondition(Condition.StringEqual("a", "11"));//在这可以加个断点，然后用工具去改a的值为22
            tran.AddCondition(Condition.StringEqual("b", "11"));
            tran.AddCondition(Condition.StringEqual("c", "11"));//相当于Watch
             
            tran.StringSetAsync("a", "33");
            tran.StringSetAsync("b", "33");
            tran.StringSetAsync("c", "33");

            System.Threading.Thread.Sleep(4000);
            flag = tran.Execute();
            Console.WriteLine($"提交结果：{flag}，获取值a:{database.StringGet("a")},b:{database.StringGet("b")},c:{database.StringGet("c")}");

            Console.ReadLine();

        }
    }
}
