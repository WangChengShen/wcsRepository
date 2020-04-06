using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Models;

namespace Wcs.BLL
{
    public interface IUserBLL
    {
        int AddUser(UserModel user);

        int AddUser_Property(UserModel user);

        int AddUser_Method(UserModel user);
    }
}
