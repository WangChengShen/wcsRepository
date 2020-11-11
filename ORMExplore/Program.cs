using Newtonsoft.Json;
using System;
using System.Threading;
using Wcs.Common;
using Wcs.Models;

namespace ORMExplore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            #region 封装ORM
            //CompanyModel company = SqlHelper.Find<CompanyModel>(1);

            //CompanyModel companyModel = new CompanyModel
            //{
            //    LinkMan = "王二",
            //    CName = "速派",
            //    Address = "上海",
            //    CreateTime = DateTime.Now,
            //    Handset = "15110011001"
            //};

            //int companyId = SqlHelper.Insert<CompanyModel>(companyModel);
            //companyModel.Id = companyId;
            //companyModel.Address = "北京";

            //bool result = SqlHelper.Update<CompanyModel>(companyModel);

            //result = SqlHelper.Delete<CompanyModel>(companyId);

            //Console.WriteLine(JsonConvert.SerializeObject(company));

            #endregion

            #region 读写分离
            //CompanyModel companyModel = new CompanyModel
            //{
            //    LinkMan = "王二",
            //    CName = "速派",
            //    Address = "上海",
            //    CreateTime = DateTime.Now,
            //    Handset = "15110011001"
            //};
            //int companyId = SqlHelper.Insert<CompanyModel>(companyModel);
            //for (int i = 0; i < 100; i++)
            //{
            //    CompanyModel company = SqlHelper.Find<CompanyModel>(companyId);
            //    if (company == null)
            //    {
            //        Console.WriteLine($"keep moving {i}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"第 {i}* 500毫秒 次查询，完成同步");
            //        break;
            //    }
            //    Thread.Sleep(500);
            //}
            #endregion

            #region 数据验证
            CompanyModel companyModel = new CompanyModel
            {
                LinkMan = "王二",
                CName = "速派!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
                Address = "上海",
                CreateTime = DateTime.Now,
                Handset = "15110011001"
            };

            bool result = ValidataExtend.Validate<CompanyModel>(companyModel, out string errorMsg);
            Console.WriteLine($"验证结果：{result}，错误信息：{errorMsg}");

            CompanyModel companyModel2 = new CompanyModel
            {
                LinkMan = "王二",
                CName = "速派12",
                Address = "上海",
                CreateTime = DateTime.Now,
                Handset = "151100110012"
            };

            bool result2 = ValidataExtend.Validate<CompanyModel>(companyModel2, out string errorMsg2);

            Console.WriteLine($"验证结果：{result2}，错误信息：{errorMsg2}");
            #endregion


            #region 按需更新
            //CompanyModel companyModel = new CompanyModel
            //{
            //    LinkMan = "王二",
            //    CName = "速派12",
            //    Address = "上海",
            //    CreateTime = DateTime.Now,
            //    Handset = "151100110012"
            //};

            //SqlHelper.Update<CompanyModel>(11, JsonConvert.SerializeObject(new
            //{
            //    CName= "速派666",
            //    Address = "上海111"
            //}));

            #endregion

            #region 模拟EF的DBContext做延迟式的SqlHelper
            //CompanyModel companyModel = new CompanyModel
            //{
            //    LinkMan = "王二",
            //    CName = "速派XXXX",
            //    Address = "上海",
            //    CreateTime = DateTime.Now,
            //    Handset = "151100110012"
            //};
            //CompanyModel companyModel2 = new CompanyModel
            //{
            //    LinkMan = "王二",
            //    CName = "速派XXXX速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12",
            //    Address = "上海",
            //    CreateTime = DateTime.Now,
            //    Handset = "151100110012"
            //};

            //// companyModel2.CName = "速派XXXX2";
            //using (SqlHelperDelay sqlHelperDelay = new SqlHelperDelay())
            //{
            //    sqlHelperDelay.Insert<CompanyModel>(companyModel);

            //    sqlHelperDelay.Insert<CompanyModel>(companyModel2);

            //    sqlHelperDelay.SaveChange();
            //}
            #endregion

            #region 用TranscationScope 来给单数据库不同SqlConnectionn连接对象提交做事务，所以TranscationScope可以在BLL里面写
            // BLL.InsertCompnay();
            #endregion


            #region 用TranscationScope 也可以给不同数据库的SqlConnectionn连接对象提交做事务， 但是.Net Core2.1之后不支持了，在方freamwork里面都是可以的
            /*
             用TranscationScope 也可以给不同数据库的SqlConnectionn连接对象提交做事务， 但是.Net Core2.1之后不支持了，在方freamwork里面都是可以的;
             做法是要打开一个windows 服务，名称叫Distributed Transaction Coordinator，打开之后就可以了，多次操作里面库字符串可夸库；
             属于分布式事务
             */
            //在这里不会成功，因为在.Net Core2.1之后不支持了，可在framework框架里面测试，不要忘了打开DTC服务
            BLL.InsertCompnayNoSameData();
            #endregion

            Console.ReadLine();
        }
    }
}
