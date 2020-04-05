using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.FactoryPattern
{
    public class Dog : IAnimal
    {
        public void Run()
        {
            Console.WriteLine($"{this.GetType().Name} 在跑");
        }


    }
}
