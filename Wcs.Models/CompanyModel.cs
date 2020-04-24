using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Common;

namespace Wcs.Models
{ 
    [WcsORMTableName("Company")]
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string LinkMan { get; set; }
        public string Handset { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
