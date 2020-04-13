using System;
using System.Collections.Generic;
using System.Text;
using Wcs.BLL.Interface;
using Wcs.DAL;
using Wcs.Models;

namespace Wcs.BLL
{
    public class StudentBLL : IStudentBLL
    {
        private readonly IStudentDAL istudentDAL;
        public StudentBLL(IStudentDAL istudentDAL)
        {
            this.istudentDAL = istudentDAL;
        }
        public void Run(StudentModel model)
        {
            istudentDAL.Run(model);
        }

        public void Study(StudentModel model)
        {
            istudentDAL.Study(model);
        }
    }
}
