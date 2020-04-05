using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ChainOfResponsibilityPattern
{
    /// <summary>
    /// 主管
    /// </summary>
    public class Manager : AbstractAuditer
    { 
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine($"请求有{base.AuditerName} 审核");
            if (context.Hour <= 24)
            {
                context.AuditResult = true;
                context.AuditRemark = "去享受你的假期吧";
            }
            else
            {
                //AbstractAuditer chief = new Chief()
                //{
                //    AuditerName = "Chief"
                //};
                //chief.Audit(context);

                base.AuditNext(context);
            }
        }


       
    }
}
