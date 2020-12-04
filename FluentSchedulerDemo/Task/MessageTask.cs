using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSchedulerDemo.Task
{
    /// <summary>
    /// 每个任务继承IJob,实现Execute，在该方法里面写代码逻辑
    /// </summary>
    public class MessageTask : IJob
    {
        public void Execute()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发送一条消息");
        }
    }
}
