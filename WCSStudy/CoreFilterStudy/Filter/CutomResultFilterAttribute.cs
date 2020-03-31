using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 结果过滤器
    /// 继承IResultFilter
    /// </summary>
    public class CutomResultFilterAttribute : Attribute, IResultFilter, IOrderedFilter, IFilterMetadata
    {
        public int Order { get; set; }

        /// <summary>
        /// 结果返回后执行
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"this is {typeof(CutomResultFilterAttribute)} OnActionExecuted ");
        }

        /// <summary>
        /// 结果返回前执行
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"this is {typeof(CutomResultFilterAttribute)} OnResultExecuting ");
        }
    }
}
