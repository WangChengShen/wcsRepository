using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{
    public class WcsHandsetAttribute : ValidataBaseAttribute
    {
        private int _length = 0;

        public WcsHandsetAttribute(int length, string errorMsg) : base(errorMsg)
        {
            this._length = length;
            this.ErrorMsg = errorMsg;
        }

        public override bool IsValid(object value)
        {
            return value != null
                && value.ToString().Length == _length;
        }


    }
}
