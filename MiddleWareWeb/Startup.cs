using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleWareWeb.utility;

namespace MiddleWareWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region 中间价俄罗斯套娃执行 
            //app.Use(next =>
            //{

            //    return async context =>
            //    {
            //        await context.Response.WriteAsync(" this is middleware 1 begin");
            //        await next(context);
            //        await context.Response.WriteAsync(" this is middleware 1 end");
            //    };
            //});
            //app.Use(next =>
            //{

            //    return async context =>
            //    {
            //        await context.Response.WriteAsync(" this is middleware 2 begin");
            //        // await next(context);
            //        await context.Response.WriteAsync(" this is middleware 2 end");
            //    };
            //});

            //另一种简单写法
            //app.Use(async (context, next) =>
            //{ 
            //    await context.Response.WriteAsync(" this is middleware 1 begin");
            //    await next();
            //    await context.Response.WriteAsync(" this is middleware 1 end");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync(" this is middleware 2 begin");
            //    //await next();
            //    await context.Response.WriteAsync(" this is middleware 2 end");
            //});
            #endregion
            #region  权限认证 

            //app.Use(next =>
            //{
            //    return async context =>
            //    {
            //        //简单模拟应用，实际逻辑没这么简单
            //        if (context.Request.Query.ContainsKey("name"))
            //        {
            //            await next(context);
            //        }
            //        else
            //        {
            //            await context.Response.WriteAsync("no authorization");
            //        }
            //    };
            //});
            #endregion

            #region 防盗链
            //使用1：用UseMiddleware方法加入自己的中间件
            //  app.UseMiddleware<RefuseStealingMiddleWare>();
            //使用2：用扩展方法，比较优雅
            app.UseRefuseStealing();
            #endregion


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
