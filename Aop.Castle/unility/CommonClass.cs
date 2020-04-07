using System;
using System.Collections.Generic;
using System.Text;

namespace Aop.Castle
{
    public class CommonClass
    {
        public virtual void MethodInterceptor()
        {
            Console.WriteLine("this is Interceptor");
        }

        public void MethodNoInterceptor()
        {
            Console.WriteLine("this is whihout Interceptor");
        }

    }
}
