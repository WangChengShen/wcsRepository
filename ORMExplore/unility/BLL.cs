using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Wcs.Models;

namespace ORMExplore
{
    public class BLL
    {
        public static bool InsertCompnay()
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                CompanyModel companyModel = new CompanyModel
                {
                    LinkMan = "王二",
                    CName = "速派XXXX",
                    Address = "上海",
                    CreateTime = DateTime.Now,
                    Handset = "151100110012"
                };
                CompanyModel companyModel2 = new CompanyModel
                {
                    LinkMan = "王二",
                    CName = "速派XXXX速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12",
                    Address = "上海",
                    CreateTime = DateTime.Now,
                    Handset = "151100110012"
                };

                // companyModel2.CName = "速派XXXX2";

                SqlHelper.Insert<CompanyModel>(companyModel);
                SqlHelper.Insert<CompanyModel>(companyModel2);

                transactionScope.Complete();
                return true;

            } 
        }

        public static bool InsertCompnayNoSameData()
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                CompanyModel companyModel = new CompanyModel
                {
                    LinkMan = "王二",
                    CName = "速派XXXX",
                    Address = "上海",
                    CreateTime = DateTime.Now,
                    Handset = "151100110012"
                };
                CompanyModel companyModel2 = new CompanyModel
                {
                    LinkMan = "王二",
                    CName = "速派XXXX速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12速派12",
                    Address = "上海",
                    CreateTime = DateTime.Now,
                    Handset = "151100110012"
                };

                // companyModel2.CName = "速派XXXX2";

                SqlHelper.Insert<CompanyModel>(companyModel);

                string sqlConn = @"Data Source=DESKTOP-GCL6M23\\WCSSQL;Initial Catalog=Wcs.DbCopy;User ID=sa;Password=123456;timeout=14400;";
                SqlHelper.InsertOtherDatabase<CompanyModel>(companyModel2, sqlConn);

                transactionScope.Complete();
                return true;

            }
        }
    }
}
