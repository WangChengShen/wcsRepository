using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Common;

namespace Wcs.Models
{
    [WcsORMTableName("Company")]
    public class CompanyModel : BaseModel
    {
        //public int Id { get; set; }

        [WcsORMProperty("CompanyName")]
        [WcsRequired("{0}不能为空"), WcsLenth(4, 14, "{0}长度错误")] //属性的横排摆放
        //[WcsLenth(4, 14, "{0}长度错误")]
        //[WcsRequired("{0}不能为空")]
        public string CName { get; set; }
        public string LinkMan { get; set; }
        [WcsHandset(11, "{0}格式错误")]
        public string Handset { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class BaseModel
    {
        [WcsORMKey]
        public int Id { get; set; }
    }
}
