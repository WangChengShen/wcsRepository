using CustomIOC.Unility;
using System;
using Wcs.BLL;
using Wcs.Common;
using Wcs.DAL;

namespace CustomIOC
{
    /// <summary>
    /// 手写IOC容器
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********手写IOC容器*************");

            CustomIOCContainer container = new CustomIOCContainer();

            container.Register<IUserDAL, UserDAL>();
            container.Register<IUserBLL, UserBLL>();
            //单接口多实现
            container.Register<IMessageDAL, SmsDAL>("smsDAL");
            container.Register<IMessageDAL, EmailDAL>("emailDAL");
            container.Register<IStudentDAL, StudentDAL>();

            //解析
            IUserDAL iuserDAL = container.Resolve<IUserDAL>();
            IUserBLL iuserBLL = container.Resolve<IUserBLL>();//解析IUserBLL需要参数
            IMessageDAL ismsDAL = container.Resolve<IMessageDAL>("smsDAL");
            IMessageDAL iemailDAL = container.Resolve<IMessageDAL>("emailDAL");

            //执行DAL方法
            iuserDAL.AddUser(new Wcs.Models.UserModel());

            //执行BLL方法
            iuserBLL.AddUser(new Wcs.Models.UserModel());

            //属性注入的方式--执行BLL方法
            iuserBLL.AddUser_Property(new Wcs.Models.UserModel());

            //方法注入的方式--执行BLL方法
            iuserBLL.AddUser_Method(new Wcs.Models.UserModel());

            //实现单接口多实现,注册或解析时要区分下字典里面的值，所以key加要shortName
            ismsDAL.Send();
            iemailDAL.Send();

            /*思考：单接口多实现如果在构造别的类的时候需要构造IMessageDAL，此时应该选择SmsDAL还是EmailDAL
             所以就要在构造函数里面用属性去指定shortName,用UserBLL做例子来实现
             在此还要引入一个新的知识点，参数也是可以打标记的（WcsParameterAttribute）
             */
            IUserBLL iuserBLL2 = container.Resolve<IUserBLL>();//解析IUserBLL需要参数

            /*
             IOC+Aop 利用Castle手写实现
             */

            Console.WriteLine("*********************IOC+Aop 利用Castle手写实现*********************");
            IStudentDAL iStudentDAL = container.Resolve<IStudentDAL>();

            //用扩展方法实现Aop,这样对这个接口里面的所有的发方法都会前后加逻辑
            IStudentDAL istudentDAL_aop = (IStudentDAL)iStudentDAL.AOP(typeof(IStudentDAL));
            istudentDAL_aop.Study(new Wcs.Models.StudentModel() { Name = "张三" });
            istudentDAL_aop.Run(new Wcs.Models.StudentModel() { Name = "张三" });

            /*这样扩展出的Aop实现，有局限性，所有的逻辑就都会在IocIntercetor的方法里面
             为了更灵活，实现MVC过滤器的那样灵活，就要引入特性,请看 public static class AopExtend的AOP扩展方法
             */

            /*
             AOP方法封装之后，可以直接和IOC融合，把CustomIOCContainer的Resolve方法返回实体的时候
             后面加上.AOP方法即可，这样整个框架就都支持AOP了,
             把上面调用AOP可以直接注释掉，在下面进行验证
             */
            iStudentDAL.Run(new Wcs.Models.StudentModel() { Name = "张三" });

        }
    }
}
