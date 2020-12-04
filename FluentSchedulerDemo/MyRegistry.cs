using FluentScheduler;
using FluentSchedulerDemo.Task;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSchedulerDemo
{
    /// <summary>
    /// 定义策略，注册任务
    /// 在构造函数里面定义策略
    /// </summary>
    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            //每天0点0分触发一次
            //Schedule<MessageTask>().ToRunEvery(1).Days().At(0, 0);

            //每分钟触发一次
            Schedule<MessageTask>().ToRunNow().AndEvery(1).Minutes();

            //每月的第一个周一的15：00触发
            //Schedule<MessageTask>().ToRunEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(15,0);

            //时间点触发
            //DateTime dateTime = Convert.ToDateTime("2020-12-04 16:55:00");
            //Schedule<MessageTask>().ToRunOnceAt(dateTime);

            //延迟一段时间后执行某任务
            //Schedule<MessageTask>().ToRunOnceAt(DateTime.Now.AddSeconds(2));

        }
    }
}
