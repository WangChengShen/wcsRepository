using System;
using System.Collections.Generic;
using System.Text;
using Wcs.DAL;
using Wcs.Models;

namespace Wcs.DAL
{
    public class StudentDAL: IStudentDAL
    {
        public void Study(StudentModel model)
        {
            Console.WriteLine("{0}在学习", model.Name);
        }

        public void Run(StudentModel model)
        {
            Console.WriteLine("学生{0}在跑步",model.Name);
        }
    }
}
