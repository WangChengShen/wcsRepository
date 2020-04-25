using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WcsORMTableNameAttribute : WcsORMMapingAttribute
    {
        //public string TableName { get; set; }
        //public WcsORMTableNameAttribute(string TableName)
        //{
        //    this.TableName = TableName;
        //}

        public WcsORMTableNameAttribute(string TableName) :base(TableName)
        {

        } 
    } 
}
