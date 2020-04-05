using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesinerPattern.SingletonPattern
{
    public class SingletonSecond
    {
        private SingletonSecond() 
        {
            Console.WriteLine("实例化了一个SingletonSecond对象2");
        }
        private static SingletonSecond singleton = null;
        static SingletonSecond()//静态构造函数，特点是：第一次使用这个类之前，一定会而且只会执行一次；
        {
            singleton = new SingletonSecond();
            Console.WriteLine("实例化了一个SingletonSecond对象");
        }
       
        public static SingletonSecond CreateInstance()
        { 
            return singleton;
        }

    }
}
