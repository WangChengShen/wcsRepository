using FluentScheduler;
using System;

namespace FluentSchedulerDemo
{
    /// <summary>
    /// FluentScheduler 定时插件的简单使用
    /// 1.nuget引入FluentScheduler包
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("now:"+DateTime.Now.ToLongTimeString());

            //初始化
            JobManager.Initialize(new MyRegistry());

            Console.Read();
        }


    }
}
