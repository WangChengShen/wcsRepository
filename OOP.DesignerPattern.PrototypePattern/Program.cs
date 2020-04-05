using System;

namespace OOP.DesignerPattern.PrototypePattern
{
    class Program
    {
        /// <summary>
        /// 原型模式，和单例模式类似；
        /// 一个类用了单例，但是又想出现另一个实例，可以用克隆的方法不走构造函数进行克隆出一个实例
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("*************原型模式************");

            Prototype prototype = Prototype.CreateInstance();
            prototype.Name = "张三"; 
            Prototype prototype2 = Prototype.CreateInstance();
            prototype2.Name = "李四";

            Console.WriteLine(prototype.Name);
            Console.WriteLine(prototype2.Name);

            /*原型模式利用MemberwiseClone进行克隆对象，这种属于是浅拷贝；
             * 浅拷贝：拷贝值类型+引用类型的引用
               深拷贝：拷贝值类型+引用类型+引用类型的志
             */
        }
    }
}
