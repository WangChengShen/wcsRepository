using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ChainOfResponsibilityPattern
{
    public abstract class AbstractAuditer
    {
        public string AuditerName { get; set; }
        public abstract void Audit(ApplyContext context);

        private AbstractAuditer nextAuditer = null;
        /// <summary>
        /// 设置下一级
        /// </summary>
        /// <param name="nextAuditer"></param>
        public void SerNext(AbstractAuditer nextAuditer)
        {
            this.nextAuditer = nextAuditer;
        }

        /// <summary>
        /// 下一级审核，放在父类里面，继承的子类也就都可以拥有
        /// </summary>
        /// <param name="context"></param>
        protected void AuditNext(ApplyContext context)
        {
            nextAuditer?.Audit(context);
        }
    }
}
