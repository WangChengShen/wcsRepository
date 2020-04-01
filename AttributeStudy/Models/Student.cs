using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttributeStudy.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "年龄")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(10, 20, ErrorMessage = "{0}范围不正确")]
        public int Age { get; set; }

        [Display(Name = "手机号")]
        [HandVallidation(ErrorMessage = "{0}错误")]
        public string Handset { get; set; }
    }
}
