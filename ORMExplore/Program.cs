using Newtonsoft.Json;
using System;
using Wcs.Models;

namespace ORMExplore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CompanyModel company = SqlHelper.Find<CompanyModel>(1);

            CompanyModel companyModel = new CompanyModel
            {
                LinkMan = "王二",
                CName = "速派",
                Address = "上海",
                CreateTime = DateTime.Now,
                Handset = "15110011001"
            };

            int companyId = SqlHelper.Insert<CompanyModel>(companyModel);

            Console.WriteLine(JsonConvert.SerializeObject(company));

            Console.ReadLine();
        }
    }
}
