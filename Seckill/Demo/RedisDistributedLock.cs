using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Linq;
using StackExchange.Redis;

namespace RedisDemo
{
    /// <summary>
    /// redis实现分布式锁
    /// </summary>
    public class RedisDistributedLock
    {
        private static object obj_lock = new object();
        public static void Skills(int minute,int theadCount)
        { 
            Console.WriteLine($"在{minute}分0秒开始抢购");
            //SkillProduct();
            //return;
            var start = true;
            while (start)
            {
                if (minute == DateTime.Now.Minute)
                {
                    start = false;

                    for (int i = 0; i < theadCount; i++)
                    {
                        Thread thread = new Thread(() =>
                        {
                            SkillProduct();
                        });

                        thread.Start();
                    }
                }
            }
        }

        /// <summary>
        /// 秒杀商品操作
        /// </summary>
        public static void SkillProduct()
        {
            //V1
            {
                //int stock = GetStock();
                //if (stock <= 0)
                //{
                //    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:不好意思，您没有秒杀到");
                //    return;
                //}
                //UpdateStock();
                //Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:恭喜您，成功秒杀到商品");
            }

            //V2：（加了lock,在同一个进程里面可以防止超卖，但是开启多个进程（多个实例）的话就不行了，也会出现超卖）： 
            {
                //lock (obj_lock)
                //{
                //    int stock = GetStock();
                //    if (stock <= 0)
                //    {
                //        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:不好意思，您没有秒杀到");
                //        return;
                //    }

                //    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:恭喜您，成功秒杀到商品,商品编号：{stock}");
                //    UpdateStock();
                //}
            }

            //V3：Redis实现分布式锁（推荐）
            {
                //RedisLock redisLock = new RedisLock();
                //redisLock.Lock();

                //int stock = GetStock();
                //if (stock <= 0)
                //{
                //    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:不好意思，您没有秒杀到");
                //    redisLock.UnLock();
                //    return;
                //}
                //UpdateStock();
                //Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:恭喜您，成功秒杀到商品,商品编号：{stock}");

                //redisLock.UnLock();
            }

            //V4（推荐）
            {
                //int stock = GetStock();
                //RedisHelper.SetStringValue("stock", $"{stock}");
                long result = RedisHelper.StringDecrement("stock");
                if (result >= 0)
                {
                    //会打印出stock条抢购成功的
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:恭喜您，成功秒杀到商品,商品编号：{result}");
                }
                else
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:不好意思，您没有秒杀到");
                } 
            }
        }

        public static string connStr= @"Data Source=DESKTOP-GCL6M23\WCSSQL;Initial Catalog=Wcs.Db;User ID=sa;Password=123456;timeout=14400;";
        public static int GetStock()
        { 
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = "select top 1 stock from Product ";
                int reader = Convert.ToInt32(comm.ExecuteScalar());
                return reader;
            }
        }

        public static bool UpdateStock()
        { 
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = "update Product set Stock=Stock-1  ";
                return comm.ExecuteNonQuery() > 0;
            }
        }
        public static bool UpdateStock(long stock)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandText = $"update Product set Stock={stock}  ";
                return comm.ExecuteNonQuery() > 0;
            }
        }

    }


    public class RedisLock
    {
        /*四要素
         1.锁名
         2.加锁
         3.解锁
         4.锁超时
        */

        private ConnectionMultiplexer connectionMultiplexer = null;
        private IDatabase database = null;
        public RedisLock()
        {
            string connection = "127.0.0.1:6379";
            connectionMultiplexer = ConnectionMultiplexer.Connect(connection);
            database = connectionMultiplexer.GetDatabase(0);
        }

        /// <summary>
        /// 加锁
        /// </summary>
        public void Lock()
        {
            /*
             LockTake方法参数：
             key:锁名
             value:谁加了这把锁
             超时间：防止死锁
             */

            //锁如果加锁失败，要继续获取锁,所以要放到while里面
            while (true)
            {
                bool flag = database.LockTake("skill_lock", Thread.CurrentThread.ManagedThreadId, TimeSpan.FromSeconds(10));

                if (flag)
                {
                    break;
                }

                //为了防止循环太快，系统宕机，休息下
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void UnLock()
        {
            //解锁时也要传入value，为了防止别的线程去解锁
            database.LockRelease("skill_lock", Thread.CurrentThread.ManagedThreadId);

            //关闭资源
            RedisHelper.connectionMultiplexer.Close();
        }
    }
}
