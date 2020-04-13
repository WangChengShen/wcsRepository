using System;
using System.Collections.Generic;
using System.Text;
using Wcs.DAL;
using Wcs.Models;
using System.Linq;
namespace Wcs.DAL
{
    public class StudentDAL : IStudentDAL
    {
        public void Study(StudentModel model)
        {
            Console.WriteLine("{0}在学习", model.Name);
        }

        public void Run(StudentModel model)
        {
            Console.WriteLine("学生{0}在跑步", model.Name);
        }

        private List<StudentModel> studentList = new List<StudentModel> {
                new StudentModel(){
                   Id=1,
                    Name="张三",
                   SchoolName="清华"
                },
                 new StudentModel(){
                   Id=2,
                    Name="李四",
                   SchoolName="北大"
                }
        };
        public StudentModel GetById(int id)
        {
            return studentList.FirstOrDefault(s => s.Id == id);
        }

        public List<StudentModel> GetStudentList()
        {
            return studentList;
        }
    }
}
