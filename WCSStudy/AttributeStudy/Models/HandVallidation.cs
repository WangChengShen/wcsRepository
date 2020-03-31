using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttributeStudy.Models
{
    public class HandVallidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null && value.ToString().Length == 11)
            {
                return true;
            }
            return false;
        }
         

    }
}
