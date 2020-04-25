using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Wcs.Common.Config
{
    public static class MappingExtent
    {
        //public static string GetTableName(this Type type)
        //{
        //    if (type.IsDefined(typeof(WcsORMTableNameAttribute), true))
        //    {
        //        WcsORMTableNameAttribute attribute = type.GetCustomAttribute<WcsORMTableNameAttribute>();
        //        return attribute.TableName;
        //    }
        //    else
        //    {
        //        return type.Name;
        //    }
        //}

        //public static string GetPropertyName(this PropertyInfo property)
        //{
        //    if (property.IsDefined(typeof(WcsORMPropertyAttribute), true))
        //    {
        //        WcsORMPropertyAttribute attribute = property.GetCustomAttribute<WcsORMPropertyAttribute>();
        //        return attribute.ColumnName;
        //    }
        //    else
        //    {
        //        return property.Name;
        //    }
        //}


        /// <summary>
        /// 把GetTableName和GetPropertyName合成一个，用他们共有的父级MemberInfo进行扩展 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMappingName(this MemberInfo type)
        {
            if (type.IsDefined(typeof(WcsORMMapingAttribute), true))
            {
                WcsORMMapingAttribute attribute = type.GetCustomAttribute<WcsORMMapingAttribute>();
                return attribute._Name;
            }
            else
            {
                return type.Name;
            }
        }


    }
}
