using System;
using System.Threading.Tasks;

namespace OOP.DesinerPattern.SingletonPattern
{
    class Program
    {
        /// <summary>
        /// 单例模式，在这里有三种实现方式：
        /// 单例不会重复的new对象，但是会在内存中常驻一个对象，所以没有必要的单例，请勿单例！
        /// 单例的常用使用场景：
        /// 线程池，数据库连接池，配置文件对象，IOC容器实例
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("********单例********************");
            //for (int i = 0; i < 10; i++)
            //{ 
            //    Singleton singleton = Singleton.CreateInstance(); 
            //}

            //for (int i = 0; i < 10; i++)
            //{ 
            //    SingletonSecond singleton = SingletonSecond.CreateInstance();
            //}
            //for (int i = 0; i < 10; i++)
            //{
            //    SingletonThird singleton = SingletonThird.CreateInstance();
            //}


            for (int i = 0; i < 10; i++)
            {
                Task.Run(() =>
                {
                    Singleton singleton = Singleton.CreateInstance();
                });
            }

            for (int i = 0; i < 10; i++)
            {
                Task.Run(() =>
                {
                    SingletonSecond singleton = SingletonSecond.CreateInstance();
                });
            }
            for (int i = 0; i < 10; i++)
            {
                Task.Run(() =>
                {
                    SingletonThird singleton = SingletonThird.CreateInstance();
                });
            }
            Console.ReadLine();
        }
    }
}
