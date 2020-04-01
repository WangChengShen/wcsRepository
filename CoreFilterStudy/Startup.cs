using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFilterStudy.Filter;
using CoreFilterStudy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFilterStudy
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
            services.AddResponseCaching();
            services.AddControllersWithViews();

            //全部注册CustomExceptionFilter过滤器,此时即使CustomExceptionFilterAttribute构造是需要参数，在这里也是可以的
            //services.AddControllersWithViews(option =>
            //{
            //    option.Filters.Add(typeof(CustomExceptionFilterAttribute));
            //});  

            //当CustomExceptionFilterAttribute 要使用ServiceFilter打到方法或控制器上面时，要先注册个CustomExceptionFilterAttribute进行使用
            //使用TypeFilter把CustomExceptionFilterAttribute打到方法或控制器上时不用注册
            //通过继承IFilterTactory实现，也要在这里进行注册
            services.AddTransient<CustomExceptionFilterAttribute>();
            services.AddTransient<ErrorViewModel>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseResponseCaching();
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
