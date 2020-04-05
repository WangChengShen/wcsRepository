using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.FactoryPattern
{
  public  class FactoryMethod
    {
        public static IAnimal CreateInstance(AnimalTypeEnum animalTypeEnum)
        {
            IFactoty factoty = CreateFactoty(animalTypeEnum);
            return factoty.CreatInstance();
        }   

        public static IFactoty CreateFactoty(AnimalTypeEnum animalTypeEnum)
        { 
            switch (animalTypeEnum)
            {
                case AnimalTypeEnum.cat:
                    return new CatFactory();
                case AnimalTypeEnum.dog:
                    return new DogFactory();
                default:
                    throw new Exception("错误");
            }
        }
    }
}
