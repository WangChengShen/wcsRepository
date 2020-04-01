using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 资源过滤器，实现缓存或则对过滤器管道进行短路特别有用
    /// 主要继承IResourceFilter接口
    /// </summary>
    public class CustomResouseFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"this is {typeof(CustomResouseFilterAttribute)} OnResourceExecuted ");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"this is {typeof(CustomResouseFilterAttribute)} OnResourceExecuting ");
        }
    }
}
