using Castle.DynamicProxy;
using System;

namespace Aop.Castle
{
    /// <summary>
    /// 基于Castle实现Aop
    /// 1.nuget添加Castle.Core 包
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //1.类型的代理
            ProxyGenerator genetator = new ProxyGenerator(); //代理类
            CustomIntercetor intercepter = new CustomIntercetor();//方法前后的逻辑
            CommonClass commonClass = genetator.CreateClassProxy<CommonClass>(intercepter);//用代理去创建实体
             
            //2、通过接口进行代理


            Console.WriteLine("当前类型:{0}，父类型:{1}", commonClass.GetType(), commonClass.GetType().BaseType);
            Console.WriteLine("");

            /*在此方法的执行前和执行后可以加入相应的逻辑
             只有虚拟的方法前后才会加入相应的逻辑，代理通过重写虚拟的方法来实现
             */
            commonClass.MethodInterceptor();
            Console.WriteLine("");
            commonClass.MethodNoInterceptor();
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
