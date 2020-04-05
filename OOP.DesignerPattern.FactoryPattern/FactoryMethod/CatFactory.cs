using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.FactoryPattern
{
    public class CatFactory : IFactoty
    {
        public IAnimal CreatInstance()
        {
            return new Cat();
        }
    }
}
