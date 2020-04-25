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
        public string CName { get; set; }
        public string LinkMan { get; set; }
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
