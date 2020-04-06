using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Models;

namespace Wcs.DAL
{
    public interface IUserDAL
    {
        int AddUser(UserModel user);
    }
}
