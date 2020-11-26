using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuceneNetDemo.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime CreateTime { get; set; }
    }
}