using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ChainOfResponsibilityPattern
{
    /// <summary>
    /// 经理
    /// </summary>
   public class Charge:AbstractAuditer
    { 
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine($"请求有{base.AuditerName} 审核");
            if (context.Hour <= 16)
            {
                context.AuditResult = true;
                context.AuditRemark = "去享受你的假期吧";
            }
            else
            {
                //AbstractAuditer charge = new Manager() {
                //   AuditerName = "Manager"
                //};
                //charge.Audit(context); 

                base.AuditNext(context);
            }
        }

       
    }
}
