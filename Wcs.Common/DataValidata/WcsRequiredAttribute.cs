using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{
    public class WcsRequiredAttribute : ValidataBaseAttribute
    {
        public WcsRequiredAttribute(string errorMsg) : base(errorMsg)
        {
            this.ErrorMsg = errorMsg;
        }

        public override bool IsValid(object value)
        {
            return value != null
                && !string.IsNullOrWhiteSpace(value.ToString());
        }


    }
}
