using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttributeStudy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AttributeStudy
{
    /// <summary>
    /// 关于Startup类，如果有Startup+环境变量 类，且Program类里面的
    /// webBuilder.UseStartup<Startup>();改为： webBuilder.UseStartup(Assembly.GetExecutingAssembly().FullName);
    /// 则就会执行Startup+环境变量 类，把Startup类给覆盖点
    /// </summary>
    public class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///  环境变量是Development,则如果有了 Configure+环境变量+Services的方法后，就会覆盖掉ConfigureServices的方法
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            //单独读取某一个配置文件
            string name = Configuration.GetValue<string>("Name");
            string name2 = Configuration["Name"];
            string Default = Configuration.GetValue<string>("Logging:LogLevel:Default");

            //全部绑定：把配置文件整个绑定到一个类里面
            var appSetting = new AppSetting();
            Configuration.Bind(appSetting);

            //部分绑定
            var logging = new Logging();
            Configuration.GetSection("Logging").Bind(logging);

            //注册为服务，在Controller里面使用时注入 IOptions<AppSetting> appSetting
            services.Configure<AppSetting>(Configuration);

            //添加自定义的配置文件，并读取，读取时AddJsonFile方法参数可写路径
            var myConfig = new ConfigurationBuilder().AddJsonFile("mySetting.json").Build();
            //注册为服务
            services.Configure<Logging>(myConfig);

            services.AddControllersWithViews();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //单独读取某一个配置文件
            string name = Configuration.GetValue<string>("Name");
            string name2 = Configuration["Name"];
            string Default = Configuration.GetValue<string>("Logging:LogLevel:Default");

            //全部绑定：把配置文件整个绑定到一个类里面
            var appSetting = new AppSetting();
            Configuration.Bind(appSetting);

            //部分绑定
            var logging = new Logging();
            Configuration.GetSection("Logging").Bind(logging);

            //注册为服务，在Controller里面使用时注入 IOptions<AppSetting> appSetting
            services.Configure<AppSetting>(Configuration);

            //添加自定义的配置文件，并读取，读取时AddJsonFile方法参数可写路径
            var myConfig = new ConfigurationBuilder().AddJsonFile("mySetting.json").Build();
            //注册为服务
            services.Configure<Logging>(myConfig);

            services.AddControllersWithViews();
        }

        /// <summary>
        /// 环境变量是Development,则如果有了 Configure+环境变量的方法后，就会覆盖掉Configure方法
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
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
