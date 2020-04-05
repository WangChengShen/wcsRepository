using System;

namespace OOP.DesignerPattern.ProxyPattern
{
    class Program
    {
        /// <summary>
        /// 代理模式，原理也是包一层，不侵入原有的代码，在建立一个代理类，在代理类里面
        /// 去添加新的功能
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /*
             场景：有一个业务逻辑类，要在其外面加一些通用的功能，如加日志，加授权,加缓存
             因为业务代码也很麻烦，直接改业务代码很容易出现问题，所以可以建一个代理的类，
             在代理类里面去实现这些功能；
             */
            Console.WriteLine("Hello World!");

            {
                Console.WriteLine("**********************实际的业务的方式**********************");
                IBusiness business = new RealBusiness();
                business.calculate(1, 2);
            }

            {
                /*代理的方式*/
                Console.WriteLine("**********************代理的方式**********************");
                IBusiness business = new ProxyBusiness();
                business.calculate(1, 2);
            }


            /*
             总结：
             代理和适配器模式套路是一样的，都是包一层，都是利用组合；

              设计模式是解决问题的套路，不同的设计模式是解决不同的问题
              适配器模式解决对象适配的问题，----可能增加或减少方法
              代理模式解决的是对象的调用问题----是不允许参与业务的

              其实多个结构行设计模式的本质是一样的，都是在外面包一层，只不过招数略有不同，在不同的
              场景下给起了个不同的名字;
             */
        }
    }
}
