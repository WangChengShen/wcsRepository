using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Wcs.Common;

namespace CustomIOC.Unility
{
    public class CustomIOCContainer
    {
        private Dictionary<string, Type> dic = new Dictionary<string, Type>();

        public string GetKey(string fullName, string shortName) => $"{fullName}__{shortName}";

        public void Register<TFrom, TTo>(string shortName = "") where TTo : TFrom
        {
            string key = GetKey(typeof(TFrom).FullName, shortName);
            dic.Add(key, typeof(TTo));
        }

        //public TFrom Resolve<TFrom>()
        //{
        //    string key = typeof(TFrom).FullName;
        //    Type type = dic[key];

        //    #region 因为构造的时候，可能构造函数有参数，所以要准备参数，然后传入
        //    List<object> paraList = new List<object>();
        //    var constructors = type.GetConstructors()[0];//简单点，找第一个
        //    foreach (var para in constructors.GetParameters())
        //    {
        //        Type paraType = para.ParameterType;//获取参数的类别，IUserDAL
        //        string paraKey = paraType.FullName;//IUserDAL的完整名称
        //        Type paraTargeType = dic[paraKey]; //UserDAL
        //        paraList.Add(Activator.CreateInstance(paraTargeType));
        //    } 
        //    #endregion

        //    return (TFrom)Activator.CreateInstance(type, paraList.ToArray());
        //}


        /*思考：如果在构造参数类的时候，它也需要其他的类实体参数，这样就要去构造这个参数，
         如果构造它的时候又需要其他类的参数，这样就无限循环下去了，所以在这里可以使用递归
         代码升级下如下:
         */

        public TFrom Resolve<TFrom>(string shortName = "")
        {
            //string key = typeof(TFrom).FullName;
            //Type type = dic[key];

            return (TFrom)ResolveObject(typeof(TFrom), shortName);
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private object ResolveObject(Type t, string shortName = "")
        {
            string key = GetKey(t.FullName, shortName);
            Type type = dic[key];

            #region 选择合适的构造函数
            /*有些类的构造函数有多个，这个时候就要选择一个进行了，
             像Core内置的ServiceCollection容器，他是根据并集原则，就是选择参数类别比较多的，用这个构造函数进行构造；
             市场上主流的第三方的容器是先找打标记的，没有的话找参数最多的构造函数
             */
            // var constructors = type.GetConstructors()[0];//简单点，找第一个

            /*我们找打了WcsConstructorAttribute标记的,没有的话找参数最多的*/

            ConstructorInfo constructor = null;
            constructor = type.GetConstructors().FirstOrDefault(f => f.IsDefined(typeof(WcsConstructorAttribute), true));
            if (constructor == null)
            {
                constructor = type.GetConstructors().OrderByDescending(f => f.GetParameters().Length).FirstOrDefault();
            }
            #endregion

            #region 因为构造的时候，可能构造函数有参数，所以要准备参数，然后传入
            List<object> paraList = new List<object>();
            foreach (var para in constructor.GetParameters())
            {
                Type paraType = para.ParameterType;//获取参数的类别，IUserDAL
                //string paraKey = paraType.FullName;//IUserDAL的完整名称
                //Type paraTargeType = dic[paraKey]; //UserDAL
                //paraList.Add(Activator.CreateInstance(paraTargeType));

                //单接口多实现，构造所需要的参数时实例化时是否需要选择构造哪个实例
                string paraShortName = GetShortName(para);

                object paraObj = this.ResolveObject(paraType, paraShortName);
                paraList.Add(paraObj);
            }
            #endregion

            object instance = Activator.CreateInstance(type, paraList.ToArray());

            #region 属性注入
            /*思路：
             * 找出构造出的对象的属性，构造属性，然后对其属性赋值，
             * 我们就只找打了WcsPropertyAttribute特性的属性
             */
            foreach (var prop in type.GetProperties().Where(p => p.IsDefined(typeof(WcsPropertyAttribute), true)))
            {
                Type propertyType = prop.PropertyType;
                object propertyInstance = ResolveObject(propertyType);
                prop.SetValue(instance, propertyInstance);
            }
            #endregion

            #region 方法注入
            /*找打了WcsMethodAttribute的方法，对他的参数进行构造*/
            foreach (var method in type.GetMethods().Where(p => p.IsDefined(typeof(WcsMethodAttribute), true)))
            {
                List<object> methodParaList = new List<object>();
                foreach (var para in method.GetParameters())
                {
                    Type paraType = para.ParameterType;
                    object paraObj = this.ResolveObject(paraType);
                    methodParaList.Add(paraObj);
                }
                method.Invoke(instance, methodParaList.ToArray());
            }
            #endregion

            // return instance;
            return instance.AOP(t);
        }


        public string GetShortName(ParameterInfo parameterInfo)
        {
            if (parameterInfo.IsDefined(typeof(WcsParameterAttribute), true))
            {
                return parameterInfo.GetCustomAttribute<WcsParameterAttribute>().shortName;
            }
            else
                return null;
        }
    }
}


