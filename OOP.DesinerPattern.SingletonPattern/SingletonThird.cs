using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesinerPattern.SingletonPattern
{
    public class SingletonThird
    {
        private SingletonThird()
        {
            Console.WriteLine("在这里实例化了一个SingletonThird对象");
        }
        /// <summary>
        /// 静态的类程序在运行时就会分配好内存，且只会实例化一次；
        /// </summary>
        private static SingletonThird singleton = new SingletonThird();

        public static SingletonThird CreateInstance()
        {
            return singleton;
        }
    }
}
