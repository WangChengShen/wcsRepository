using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ProxyPattern
{
    public class ProxyBusiness : IBusiness
    {
        /// <summary>
        /// 如果是要实现单例代理，即RealBusiness只初始化一次，在前面加Static即可
        /// </summary>
        private static IBusiness _ibusiness = new RealBusiness();
        public int calculate(int a, int b)
        {
            Console.WriteLine("前面加日志");
            Console.WriteLine("前面加授权");
            int result= _ibusiness.calculate(a, b);
            Console.WriteLine("后面加日志");

            return result;
        }
    }
}
