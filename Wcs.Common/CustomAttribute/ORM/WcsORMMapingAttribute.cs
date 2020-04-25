using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{ 
    /// <summary>
    /// 声明一个abstract的父级属性
    /// </summary>
    public abstract class WcsORMMapingAttribute : Attribute
    {
        public string _Name { get; set; }
        public WcsORMMapingAttribute(string Name)
        {
            this._Name = Name;
        }
    }

}
