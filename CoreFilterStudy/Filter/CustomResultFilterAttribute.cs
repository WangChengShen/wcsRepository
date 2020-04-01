using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 基于ResultFilter来实现ResposeCache
    /// </summary>
    public class CustomResultFilterAttribute : Attribute, IResultFilter
    {
        //基于构造函数
        //private int Duration { get; set; }
        //public CustomResultFilterAttribute(int Duration)
        //{
        //    this.Duration = Duration;
        //}

        //基于属性
        public int Duration { get; set; }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            //这里不行，已经指定了Respose
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = $"public,max-age={this.Duration}";
        }
    }
}
