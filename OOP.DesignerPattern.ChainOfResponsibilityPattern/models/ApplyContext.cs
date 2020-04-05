using System;
using System.Collections.Generic;
using System.Text;

namespace OOP.DesignerPattern.ChainOfResponsibilityPattern
{
    /// <summary>
    /// 请假申请
    /// </summary>
    public class ApplyContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 请假时长
        /// </summary>
        public int Hour { get; set; }

        public string Description { get; set; }
        public bool AuditResult { get; set; }
        public string AuditRemark { get; set; }
    }
}
