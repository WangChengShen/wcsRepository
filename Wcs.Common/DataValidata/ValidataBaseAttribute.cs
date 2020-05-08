using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidataBaseAttribute : Attribute
    {
        public ValidataBaseAttribute(string errorMsg)
        {
            this.ErrorMsg = errorMsg;
        }
        public string ErrorMsg
        {
            get; set;
        }

        public abstract bool IsValid(object value);

    }
}
