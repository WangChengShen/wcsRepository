using System;
using Wcs.Common;
using Wcs.DAL;
using Wcs.Models;

namespace Wcs.BLL
{
    public class UserBLL : IUserBLL
    {
        /// <summary>
        /// 构造函数注入
        /// </summary>
        private readonly IUserDAL iUbserDAl;

        private readonly IMessageDAL messageDAL;

        public UserBLL(IUserDAL iUbserDAl, [WcsParameterAttribute(shortName: "emailDAL")]IMessageDAL messageDAL)
        {
            this.iUbserDAl = iUbserDAl;
            this.messageDAL = messageDAL;
        }

        /// <summary>
        /// 属性注入
        /// </summary>
        [WcsPropertyAttribute]
        public IUserDAL iuserDAL_property { get; set; }

        public int AddUser(UserModel user)
        {
            return iUbserDAl.AddUser(user);
        }

        public int AddUser_Property(UserModel user)
        {
            return iuserDAL_property.AddUser(user);
        }

        /// <summary>
        /// 方法注入
        /// </summary>
        private IUserDAL iuserDAL_method;
        [WcsMethodAttribute]
        public void MethodDI(IUserDAL iuserDAL_method)
        {
            this.iuserDAL_method = iuserDAL_method;
        }
        public int AddUser_Method(UserModel user)
        {
            return iuserDAL_method.AddUser(user);
        }


    }
}
