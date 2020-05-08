using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Wcs.Common
{
    public class ValidataExtend
    {
        public static bool Validate<T>(T t, out string errorMsg)
        {
            errorMsg = string.Empty;
            Type type = typeof(T);

            type.GetCustomAttributes(typeof(WcsLenthAttribute), true);

            bool valid = true;
            foreach (PropertyInfo itemPropertyInfo in type.GetProperties())
            {
                //if (itemPropertyInfo.IsDefined(typeof(WcsLenthAttribute)))
                //{
                //    WcsLenthAttribute wcsLenthAttribute = itemPropertyInfo.GetCustomAttribute<WcsLenthAttribute>();
                //    if (wcsLenthAttribute != null)
                //    {
                //        if (!wcsLenthAttribute.IsValid(itemPropertyInfo.GetValue(t)))
                //        {
                //            errorMsg = string.Format(wcsLenthAttribute.ErrorMsg, itemPropertyInfo.Name);
                //            valid = false;
                //            break;
                //        }
                //    }
                //}

                //升级下
                if (itemPropertyInfo.IsDefined(typeof(ValidataBaseAttribute)))
                {
                    var wcsLenthAttributeArray = itemPropertyInfo.GetCustomAttributes<ValidataBaseAttribute>();

                    foreach (var itemAttribute in wcsLenthAttributeArray)
                    {
                        if (!itemAttribute.IsValid(itemPropertyInfo.GetValue(t)))
                        {
                            errorMsg = string.Format(itemAttribute.ErrorMsg, itemPropertyInfo.Name);
                            valid = false;
                            break;
                        }
                    }
                   
                }
            }
            return valid;
        }

    }
}
