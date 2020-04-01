using Microsoft.AspNetCore.Mvc;
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


    /// <summary>
    /// 根据TestOrder的action方法得出结论：ResourceFilter过滤器是在Controller的构造之前执行的，是Core独有的，.NetFramwork是没有的
    /// 所以他可以用来做缓存
    /// </summary>
    public class CustomCacheResourceFilterAttribute : Attribute, IResourceFilter
    {
        private static Dictionary<string, IActionResult> cacheDic = new Dictionary<string, IActionResult>();

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string key = context.HttpContext.Request.Path;
            if (cacheDic.Keys.Contains(key))
            {
                context.Result = cacheDic[key];
            } 
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            string key = context.HttpContext.Request.Path;
            cacheDic.Add(key, context.Result);
        }


    }

}
