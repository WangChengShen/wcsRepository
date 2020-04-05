using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.PrototypePattern
{
    public class Prototype
    {
        private Prototype()
        {
            Console.WriteLine("Prototype被构造了一次");
        }

        public string Name { get; set; }

        private static Prototype _prototype = new Prototype();
        public static Prototype CreateInstance()
        {
            Prototype prototype = (Prototype)_prototype.MemberwiseClone();
            return prototype;
        }
    }
}
