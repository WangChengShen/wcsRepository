using Microsoft.AspNetCore.Http;
using System;

namespace MiddleWareFlow
{
    class Program
    {
        static void Main(string[] args)
        { 
            //Core 控制台读取配置文件
            string name = ConfigurationManager.GetNote("Name");
            string defaultLog = ConfigurationManager.GetNote("Logging:LogLevel:Default");

            var app = new ApplicationBuilder();

            //中间件的作用：实现自己的逻辑，调用下一个中间件
            app.Use(next =>
            {
                return async context =>
                {

                    Console.WriteLine("中间件1 Begin");
                    await next(context);
                    Console.WriteLine("中间件1 end");
                };
            });

            app.Use(next =>
            {
                return async context =>
                {
                    Console.WriteLine("中间件2 Begin");
                    await next(context);
                    Console.WriteLine("中间件2 end");
                };
            });

            //另一种简单写法
            //app.Use(async (context, next) =>
            //{
            //    //whiteLog(path, "中间件1号 Begin");
            //    Console.WriteLine("中间件1号 Begin");
            //    await next();
            //    //whiteLog(path, "中间件1号 End");
            //    Console.WriteLine("中间件1号 End");
            //});

            //app.Use(async (context, next) =>
            //{
            //    //whiteLog(path, "中间件2号 Begin");
            //    Console.WriteLine("中间件2号 Begin");
            //    await next();
            //    //whiteLog(path, "中间件2号 End");
            //    Console.WriteLine("中间件2号 End");
            //});



            // 这时候管道已经形成，执行第一个中间件，就会依次调用下一个
            // 主机创建以后运行的

            //Build方法Reverse后一步步仅仅是得到第一个中间件返回的一个委托，这个委托等待Kestrel监听请求把HttpContext的请求包传给这个委托，这个
            //委托触发了会接连触发一系列的每个中间件 
            RequestDelegate requestDelegate = app.Build(); //得到第一中间件返回的委托

            //在这个模拟出发
            HttpContext context = null;
            requestDelegate(context);

            Console.ReadLine();

            // 责任链模式，这里我觉得很容易就搞明白了！理解很吃力，责任链模式不了解  
            // 应用配置、路由、静态文件
            // 责任链模式应用很广的，凡是流程化的应用， 都可以这么做，我自己已经仿造ASP.NET Core写了一个流程框架。
        }
    }
}
