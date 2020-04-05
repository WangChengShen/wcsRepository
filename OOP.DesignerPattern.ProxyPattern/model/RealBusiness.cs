using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ProxyPattern
{
    public class RealBusiness : IBusiness
    {
        public int calculate(int a, int b)
        {
            int result = a + b;
            Console.WriteLine($"{a}+{b}={result}");
            return result;
        }
    }
}
