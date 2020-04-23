using ORMExplore.Model;
using System;
using Wcs.Models;

namespace ORMExplore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            StudentModel stu = SqlHelper.Find<StudentModel>(1);
        }
    }
}
