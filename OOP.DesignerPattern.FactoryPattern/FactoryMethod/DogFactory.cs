using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.FactoryPattern
{
    public class DogFactory : IFactoty
    {
        public IAnimal CreatInstance()
        {
            return new Dog();
        }
    }
}
