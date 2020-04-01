using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 方法异步过滤器
    /// </summary>
    public class CustomAsyncActionFilterAttribute : Attribute, IAsyncActionFilter, IOrderedFilter, IFilterMetadata
    {
        public int Order => throw new NotImplementedException();

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
