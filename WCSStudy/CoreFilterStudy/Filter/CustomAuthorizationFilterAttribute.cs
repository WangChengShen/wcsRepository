using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreFilterStudy.Filter
{
    /// <summary>
    /// 授权过滤器
    /// 实现：主要要继承IAuthorizationFilter接口，因为要可以打到方法或过滤器上面，所以要继承属性Attribute,
    /// IFilterMetadata 这个接口最好加上，在Core里面会用来过滤器的依赖注入（不是很懂，记着就行）
    /// IOrderedFilter  这个接口为了实现如果多个授权过滤器的话，可以根据执行Order顺序来指定执行的顺序；
    /// </summary>
    public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter, IFilterMetadata, IOrderedFilter
    {
        public int Order => throw new NotImplementedException();

        /// <summary>
        /// 授权过滤器可以用来验证登陆
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //如果打了AllowAnonymousFilter，则跳过验证登录
            if (context.Filters.Any(f => f is Microsoft.AspNetCore.Mvc.Authorization.IAllowAnonymousFilter))
            {
                return;
            }
            bool isLogin = false;
            if (isLogin)
            {
                context.Result = new RedirectResult("/Account/Login");
            }
             
            //throw new NotImplementedException();
        }
    }
}
