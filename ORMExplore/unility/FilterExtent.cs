using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wcs.Common;
using Wcs.Common.Config;

namespace ORMExplore.unility
{
    public static class FilterExtent
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithoutKey(this Type type)
        {
            return type.GetProperties().Where(t => !t.IsDefined(typeof(WcsORMKeyAttribute))).ToList();
        }
        public static IEnumerable<PropertyInfo> GetPropertiesInJson(this Type type, string json)
        {
            return type.GetProperties().Where(t => json.Contains($"'{t.Name}':") || json.Contains($"\"{t.Name}\":")).ToList();
        }
    }
}
