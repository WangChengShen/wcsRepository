using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ChainOfResponsibilityPattern
{
    /// <summary>
    /// 总监
    /// </summary>
   public class Chief : AbstractAuditer
    { 
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine($"请求有{base.AuditerName} 审核");
            if (context.Hour <= 32)
            {
                context.AuditResult = true;
                context.AuditRemark = "去享受你的假期吧";
            }
            //else
            //{
            //    AbstractAuditer charge = new Manager();
            //    charge.Audit(context); 
            //}

            else 
            {

                base.AuditNext(context);
            }
        }
 
    }
}
