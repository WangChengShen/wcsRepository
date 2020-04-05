using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesinerPattern.SingletonPattern
{
    /// <summary>
    /// 标准单例模式三板斧：
    /// 1.构造函数私有化
    /// 2.对外提供创建实例的方法
    /// 3：考虑多线程，加把锁🔒，私有对象
    /// </summary>
    public class Singleton
    {
        private Singleton()
        {
            Console.WriteLine("实例化了一个Singleton对象");
        }
        private static object object_lock = new object();
        private static Singleton _Singleton = null;

        public static Singleton CreateInstance()
        {
            if (_Singleton == null)//由于加锁也是会消耗性能的，所以在外面再判断对象是否为null,不为null了就不用再等待直接返回，为null了再进入加锁判断；
            {
                lock (object_lock)//只允许一个线程进入
                {
                    if (_Singleton == null)//判断对象是够为空 
                    {
                        _Singleton = new Singleton();
                    }
                }
            }
            return _Singleton; 
        } 
    }


}
