using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wcs.Common;

namespace ORMExplore.unility
{
    public static class FilterExtent
    { 
        public static IEnumerable<PropertyInfo> GetPropertiesWithoutKey(this Type type)
        {
            return type.GetProperties().Where(t => !t.IsDefined(typeof(WcsORMKeyAttribute))).ToList();
        }
    }
}
