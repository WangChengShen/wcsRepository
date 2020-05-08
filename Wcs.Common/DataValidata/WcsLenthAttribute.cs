using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{
    public class WcsLenthAttribute : ValidataBaseAttribute
    {
        private int _min = 0;
        private int _max = 0;
      
        public WcsLenthAttribute(int min, int max, string errorMsg):base(errorMsg)
        {
            _min = min;
            _max = max;
            this.ErrorMsg = errorMsg;
        }

        public override bool IsValid(object value)
        {
            return value != null
                && value.ToString().Length >= _min
                && value.ToString().Length <= _max;
        }

       
    }
}
