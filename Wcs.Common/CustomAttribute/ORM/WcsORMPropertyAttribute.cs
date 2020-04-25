using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{ 
    [AttributeUsage(AttributeTargets.Property)]
    public class WcsORMPropertyAttribute : WcsORMMapingAttribute
    {
        //public string ColumnName { get; set; }
        //public WcsORMPropertyAttribute(string ColumnName)
        //{
        //    this.ColumnName = ColumnName;
        //}

        public WcsORMPropertyAttribute(string ColumnName):base(ColumnName)
        {

        }
    }
     
}
