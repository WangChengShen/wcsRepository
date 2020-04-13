using System;
using System.Collections.Generic;
using System.Text; 
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

        public StudentModel GetById(int id)
        {
            return istudentDAL.GetById(id);
        }

        public List<StudentModel> GetStudentList()
        {
            return istudentDAL.GetStudentList();
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
