using System;

namespace OOP.DesignerPattern.FactoryPattern
{
    class Program
    {
        /// <summary>
        ///工程模式
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            {
                //简单工厂
                //IAnimal animal = SimpleFactory.CreateInstance(AnimalTypeEnum.dog);
                //animal.Run();

                //IAnimal animal2 = SimpleFactory.CreateInstanceByConfig();
                //animal2.Run();

                //IAnimal animal3 = SimpleFactory.CreateInstanceByReflect();
                //animal3.Run();
            }

            {
                //工厂方法
                IAnimal animal = FactoryMethod.CreateInstance(AnimalTypeEnum.dog);
                animal.Run();
            }
        }
    }
}
