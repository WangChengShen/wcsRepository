using System;
using Wcs.Models;

namespace Wcs.DAL
{
    public class UserDAL: IUserDAL
    {
        public int AddUser(UserModel user)
        {
            Console.WriteLine("添加用户");
            return 1;
        }
    }
}
