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

            Console.WriteLine(JsonConvert.SerializeObject(company));
        }
    }
}
