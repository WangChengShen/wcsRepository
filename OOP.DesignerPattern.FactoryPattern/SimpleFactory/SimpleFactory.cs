using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace OOP.DesignerPattern.FactoryPattern
{
    public class SimpleFactory
    {
        public static IAnimal CreateInstance(AnimalTypeEnum animalTypeEnum)
        {
            switch (animalTypeEnum)
            {
                case AnimalTypeEnum.cat:
                    return new Cat();
                case AnimalTypeEnum.dog:
                    return new Dog();
                default:
                    throw new Exception("错误");
            }
        }

        /// <summary>
        /// 读配置文件创建对象
        /// </summary>
        /// <returns></returns>
        public static IAnimal CreateInstanceByConfig()
        {
            string config = ConfigurationManager.AppSettings["AnimalTypeConfig"];
            AnimalTypeEnum animalTypeEnum = (AnimalTypeEnum)Enum.Parse(typeof(AnimalTypeEnum), config);

            switch (animalTypeEnum)
            {
                case AnimalTypeEnum.cat:
                    return new Cat();
                case AnimalTypeEnum.dog:
                    return new Dog();
                default:
                    throw new Exception("错误");
            }
        }


        /// <summary>
        /// 利用配置文件，反射动态加载dll创建对象，实现程序可配置高扩展性
        /// </summary>
        /// <returns></returns>
        public static IAnimal CreateInstanceByReflect()
        {
            string config = ConfigurationManager.AppSettings["AnimalTypeReflect"];

            string assemblyName = config.Split(',')[0];
            string animalType = config.Split(',')[1];

            object obj = Activator.CreateInstance(assemblyName, animalType).Unwrap();
            return (IAnimal)obj;
        }

    }

    public enum AnimalTypeEnum
    {
        dog = 1,
        cat = 2
    }
}
