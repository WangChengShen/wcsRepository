using System;

namespace AdapterPattern
{
    /// <summary>
    /// 适配器模式
    /// 主要是用在扩展功能时，有些东西和现有的接口不太匹配，然后再外面包一层，达到适配的效果（鲁迅说没有什么事
    /// 是包一层解决不了的，如果有就再包一层）包治百病
    /// 适配器模式属于补救模式，引入全新的组件时能用上，不要在新的项目上去使用适配器模式，徒增难度！！
    /// </summary>
    class Program
    {
        /*
         举一场景来分析适配器模式：
          做Ado.Net时有一接口，不通数据库操作是都要实现这个接口，现在要加一个非关系型数据库Redis，也要
          用这种形式进行调用
         */
        static void Main(string[] args)
        {
            {
                Console.WriteLine("*******************************");
                IDbHelper helper = new SqlServiceHelper();
                Program p = null;
                helper.Add<Program>(p);
                helper.Update<Program>(p);
                helper.Delete<Program>(p);
                helper.Query<Program>(p);
            }

            {
                Console.WriteLine("*******************************");
                IDbHelper helper = new MySqlHelper();
                Program p = null;
                helper.Add<Program>(p);
                helper.Update<Program>(p);
                helper.Delete<Program>(p);
                helper.Query<Program>(p);
            }

            {
                Console.WriteLine("*******************************");
                IDbHelper helper = new OracleHelper();
                Program p = null;
                helper.Add<Program>(p);
                helper.Update<Program>(p);
                helper.Delete<Program>(p);
                helper.Query<Program>(p);
            }

            {
                /* 有一个第三方Redis的类库RedisHelper的帮助类，现在要实现使用该类的方式，要像使用关系型数据库一样，现在不适配，怎么适配一下
                 */

                //1.通过继承的方式来实现,在外面包一层RedisHelperInherit，通过调用RedisHelperInherit来实现

                Console.WriteLine("*************通过继承的方式来实现,在外面包一层RedisHelperInherit，通过调用RedisHelperInherit来实现******************");
                IDbHelper helper = new RedisHelperInherit();
               
                Program p = null;
                helper.Add<Program>(p);
                helper.Update<Program>(p);
                helper.Delete<Program>(p);
                helper.Query<Program>(p);

            }
            {
                /*2.通过组合的方式来实现*/

                Console.WriteLine("*************通过组合的方式来实现,在外面包一层RedisHelperCombinnation，通过调用RedisHelperCombinnation来实现******************");
                IDbHelper helper = new RedisHelperCombinnation();
                Program p = null;
                helper.Add<Program>(p);
                helper.Update<Program>(p);
                helper.Delete<Program>(p);
                helper.Query<Program>(p);
            }

            /*分析两种方式优劣：
             * 组合优于继承
             * 1.继承是强侵入性的（有多余的东西），因为这样 new RedisHelperInherit().点后面会有AddRedis等方法，但这样方式不是我们要的；
                      new RedisHelperInherit().AddRedis
              2.灵活性问题（组合是依赖抽象的，继承是依赖细节的）
             */



        }
    }
}
