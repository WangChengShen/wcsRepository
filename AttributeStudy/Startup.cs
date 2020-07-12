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
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AttributeStudy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///  环境变量是Development,则如果有了 Configure+环境变量+Services的方法后，就会覆盖掉ConfigureServices的方法
        /// </summary>
        /// <param name="services"></param>
        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    //单独读取某一个配置文件
        //    string name = Configuration.GetValue<string>("Name");
        //    string name2 = Configuration["Name"];
        //    string Default = Configuration.GetValue<string>("Logging:LogLevel:Default");

        //    //全部绑定：把配置文件整个绑定到一个类里面
        //    var appSetting = new AppSetting();
        //    Configuration.Bind(appSetting);

        //    //部分绑定
        //    var logging = new Logging();
        //    Configuration.GetSection("Logging").Bind(logging);

        //    //注册为服务，在Controller里面使用时注入 IOptions<AppSetting> appSetting
        //    services.Configure<AppSetting>(Configuration);

        //    //添加自定义的配置文件，并读取，读取时AddJsonFile方法参数可写路径
        //    var myConfig = new ConfigurationBuilder().AddJsonFile("mySetting.json").Build();
        //    //注册为服务
        //    services.Configure<Logging>(myConfig);

        //    services.AddControllersWithViews();
        //}


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 配置文件的读取 
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
            #endregion

            #region Core内置的依赖注入 （有一定的局限性，仅支持构造函数注入）
            //Core内置的依赖注入,同一个接口注册多个实现的话，后面的会覆盖前面的
            services.AddTransient<IMessageService, SmsService>();//瞬时模式，每次调用都会new一个实例
            services.AddScoped<IMessageService, SmsService>();//作用域模式(作用域单例，每次请求使用一个实例)，每次http请求开始会new一个实例，在这次请求的下次使用则直接拿来用
            services.AddSingleton<IMessageService, SmsService>();//单例模式，整个应用都会是同一个实例
            #endregion

            #region core的自定义服务，一般这样写，比较优雅 
            //core的自定义服务，一般这样写，比较优雅
            services.AddMessage();//没有配置，使用默认的
            services.AddMessage(builder => builder.SendEmail()); //进行配置的
            #endregion

            #region Session的使用
            /*Core里面session也是定制的,使用时要在Service里面AddSession,
             然后在Configure里面调用UserSession
             */
            services.AddSession();
            #endregion

            //web项目会有这个service
            services.AddControllersWithViews(); 
        }

        /// <summary>
        /// 环境变量是Development,则如果有了 Configure+环境变量的方法后，就会覆盖掉Configure方法
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        //public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }
        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles();

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllerRoute(
        //            name: "default",
        //            pattern: "{controller=Home}/{action=Index}/{id?}");

        //    });
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
              ILoggerFactory loggerFactory)
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
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()  //指定静态文件的地址，用命令进行托管的时候，要执行下，不然找不到样式
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot")),
                //OnPrepareResponse = c =>
                //{
                //    c.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "no-cache";
                //}
            });

            //使用session
            app.UseSession();
            //自定义中间件
            // app.UseMiddleware<TestMiddleWare>();
            //或者外面再包一层，用扩展方法
            //app.UserTest();

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.StartAsync();
            //    await next();
            //    await context.Response.StartAsync();
            //});

            //app.Use(,)
             
            app.UseRouting();
             
            app.UseAuthorization();

            loggerFactory.AddLog4Net();


            //终结点中间件
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
