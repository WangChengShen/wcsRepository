using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text; 
using Newtonsoft.Json;

namespace Wcs.Common
{
    public static class AopExtend
    {
        /// <summary>
        /// 对接进行aop，这样对这个接口里面的所有的发方法都会前后加逻辑
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static object AOP(this object obj, Type interfaceType)
        {
            ProxyGenerator genetator = new ProxyGenerator(); //代理类
            IocIntercetor intercepter = new IocIntercetor();//方法前后的逻辑

            obj = genetator.CreateInterfaceProxyWithTarget(interfaceType, obj, intercepter);
            return obj;
        }

    }

    /// <summary>
    /// 建立一个抽象的特性类，这样在Castle的方法里面找特性的时候可以直接找BaseAttribute
    /// 扩展属性的时候直接继承BaseAttribute就可以了
    /// </summary>
    public abstract class BaseAttribute : Attribute
    {
        public abstract void Do(IInvocation invocation);
    }

    public class LogBeforeAttribute : BaseAttribute
    {
        public override void Do(IInvocation invocation)
        {
            Console.WriteLine("LogBeforeAttribute逻辑，Method:{0},参数：{1}", invocation.Method.Name,
              JsonConvert.SerializeObject(invocation.Arguments));
        }
    }
    public class LogAfterAttribute : BaseAttribute
    {
        public override void Do(IInvocation invocation)
        {
            Console.WriteLine("LogAfterAttribute逻辑，Method:{0},参数：{1}", invocation.Method.Name,
              JsonConvert.SerializeObject(invocation.Arguments));
        }
    }

    public class IocIntercetor : StandardInterceptor
    {
        /// <summary>
        /// 调用前的拦截器
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PreProceed(IInvocation invocation)
        {
            //Console.WriteLine("调用前的拦截器，方法名是：{0}", invocation.Method.Name);
        }

        /// <summary>
        /// 拦截的方法返回时调用的拦截器
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PerformProceed(IInvocation invocation)
        {
            //之前的代码
            //Console.WriteLine("拦截的方法返回时调用的拦截器，方法名是：{0}", invocation.Method.Name);
            //base.PerformProceed(invocation);//方法真实的动作

            var method = invocation.Method;
            if (method.IsDefined(typeof(BaseAttribute), true))
            {
                //var attribute = method.GetCustomAttribute<BaseAttribute>();
                //attribute.Do();

                foreach (var attribute in method.GetCustomAttributes<BaseAttribute>())
                {
                    attribute.Do(invocation);
                }
            }

            base.PerformProceed(invocation);
        }

        /// <summary>
        /// 调用后的拦截器
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PostProceed(IInvocation invocation)
        {
            //Console.WriteLine("调用后的拦截器，方法名是：{0}", invocation.Method.Name);
            base.PostProceed(invocation);
        }

    }
}
