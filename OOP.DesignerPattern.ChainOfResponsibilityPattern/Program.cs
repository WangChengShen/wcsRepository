using System;

namespace OOP.DesignerPattern.ChainOfResponsibilityPattern
{
    /// <summary>
    /// 使多个对象都有处理请求的机会，从而避免了请求的发送者和接受者之间的耦合关系，将这些对象串成一条链，
    /// 并沿着这条链传递该请求，直到有对象处理他为止。
    /// 责任链模式是行为型设计模式的巅峰之作，无止境的行为封装转移；
    /// 
    /// 行为型的设计模式的套路是甩锅，把不稳定的部分甩出去，只保留稳定的
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /*场景：请假审批流程
             * PM(项目组长审核8小时的)-->Charge主管审核16-->Manager（经理审核24小时）-->Chief(总监审核32)
             */
            ApplyContext context = new ApplyContext()
            {
                Id = 1,
                Name = "张三",
                Hour = 32,
                Description = "我想回家",
                AuditResult = false,
                AuditRemark = ""
            };

            {
                //下面的写法直白明了，是面向过程变成（POP）；
                //缺点：暴露业务的细节，而且毫无技术含量，代码翻译机
                //if (context.Hour < 8)
                //{
                //    Console.WriteLine("PM审核通过该");
                //}
                //else if (context.Hour < 16)
                //{
                //    Console.WriteLine("主管审核通过该");
                //}
                //else
                //{
                //    Console.WriteLine("******************");
                //}
            }
            {
                /*责任链模式实现，第一次封装*/
                //Console.WriteLine("责任链模式实现，第一次封装");

                //AbstractAuditer pm = new PM();
                //pm.Audit(context);

                //if (!context.AuditResult)
                //{
                //    AbstractAuditer charge = new Charge();
                //    charge.Audit(context);

                //    if (!context.AuditResult)
                //    {
                //        //未完待续
                //    }
                //}
            }

            {
                /*责任链模式实现，第二次封装，在角色审核方法里面添加else 后自动转给下一个角色进行审核*/
                //Console.WriteLine("责任链模式实现，第二次封装");

                //AbstractAuditer pm = new PM()
                //{
                //    AuditerName = "PM"
                //};
                //pm.Audit(context);
                //bool result = context.AuditResult;

            }

            {
                /*
                 上面的方式已经是oop的实现方式，但是也有很大的问题，就是下一级在类里面写死了，如果流程变了则要改动类
                 怎么样保证代码的稳定的性呢？ 把不稳定的部分甩出去，甩锅大法
                 代码再改动下,在虚拟父类里面添加设置下一个审批人的方法，然后改动每个子类实现该方法
                 */

                AbstractAuditer pm = new PM() { AuditerName = "PM" };
                AbstractAuditer charge = new Charge() { AuditerName = "Charge" };
                AbstractAuditer manager = new Manager() {   AuditerName = "Manager"   };
                AbstractAuditer chief = new Chief()  {    AuditerName = "Chief"   };
              
                //设置下一级
                pm.SerNext(charge); 
                charge.SerNext(manager);
                manager.SerNext(chief);

                //开始审核
                pm.Audit(context);
                bool result = context.AuditResult;
                Console.WriteLine($"result:{result}");
            }




        }
    }
}
