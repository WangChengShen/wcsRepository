using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 在Core里面更加定制化，方法过滤器的实现，可以继承IActionFilter来实现，因为要打在方法或过滤器上面，所以还要继承Attribute，
    ///  IFilterMetadata 这个接口最好加上，在Core里面会用来过滤器的依赖注入（不是很懂，记着就行）
    ///  IOrderedFilter 继承这个接口，可以指定当有多个过滤器时的执行顺序
    /// </summary>
    public class CustomActionFiilterAttribute : Attribute, IActionFilter, IFilterMetadata
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"this is {typeof(CustomActionFiilterAttribute)} OnActionExecuted ");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"this is {typeof(CustomActionFiilterAttribute)} OnActionExecuting ");
        }
    }

    /// <summary>
    /// 也可以直接继承ActionFilterAttribute来实现
    /// </summary>
    //public class CustomActionFiilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        base.OnActionExecuting(context);
    //    }

    //    public override void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        base.OnActionExecuted(context);
    //    }
    //}

    /*思考：如果把方法过滤器分别加到方法上，控制器上，全局上，会怎么执行
     可以通过以下方式验证，把CustomActionFiilterAttribute加载方法上，CustomControllerFiilterAttribute加载控制器上，
     CustomGlobalFiilterAttribute加载全局
     执行顺序如下：
         OnActionExecuting： 先全局，控制器，方法
         OnActionExecuted ：  方法,控制器,全局

        和管道模型类似，俄罗斯套娃
     */

    public class CustomControllerFiilterAttribute : Attribute, IActionFilter, IFilterMetadata
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"this is {typeof(CustomControllerFiilterAttribute)} OnActionExecuted ");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"this is {typeof(CustomControllerFiilterAttribute)} OnActionExecuting ");
        }
    }

    public class CustomGlobalFiilterAttribute : Attribute, IActionFilter, IFilterMetadata
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"this is {typeof(CustomGlobalFiilterAttribute)} OnActionExecuted ");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"this is {typeof(CustomGlobalFiilterAttribute)} OnActionExecuting ");
        }
    }

}
