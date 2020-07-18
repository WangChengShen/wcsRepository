using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wcs.DAL;
using Wcs.Models;
namespace AOC.Autofac
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
            /*
                AOC与DI（依赖注入）的区别：
                AOC是容器，是要达成的目标，具有以下优点：
                1.解耦；2：屏蔽细节
                DI：依赖注入，是一种手段，假如构造实体B时，需要实体A，则会自动构造A传入到B的构造函数，构造C需要B，则会自动构造B;
                根据递归的方式进行无限构造；

            Core里面内置的容器和第三方的注入容器可以同时使用的；
             */
            #region Core内置的依赖注入 （有一定的局限性，仅支持构造函数注入）
            //Core内置的依赖注入,同一个接口注册多个实现的话，后面的会覆盖前面的
            //services.AddTransient<IMessageService, SmsService>();//瞬时模式，每次调用都会new一个实例
            //services.AddScoped<IMessageService, SmsService>();//作用域模式(作用域单例，每次请求使用一个实例)，每次http请求开始会new一个实例，在这次请求的下次使用则直接拿来用
            //services.AddSingleton<IMessageService, SmsService>();//单例模式，整个应用都会是同一个实例
            #endregion

            #region Autofac的使用
            /*因为Core内置的依赖注入有一定的局限性，所以一般会引入第三方的IOC容器；
             Autofac的使用（在autofac的官网里面可以找到使用方法 https://autofac.readthedocs.io/en/latest/integration/aspnetcore.html）：
             1.nuget引入两个包:Autofac和Autofac.Extensions.DependencyInjection  (Autofac.Extras.DynamicProxy引用这个包后可以用autofac的aop的功能)
             2.在Program里面指定容器使用autofac
             3.添加ConfigureContainer方法，在这个放里面可以写容器的实体映射
             */
            #endregion

            services.AddControllersWithViews();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddControllersAsServices();
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

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //单个注册
            builder.RegisterType<SmsDAL>().As<IMessageDAL>();
            //builder.RegisterType<SmsDAL>().AsImplementedInterfaces();//自动找继承接口

            //批量注册依赖到接口
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.DAL")).Where(type => type.Name.EndsWith("DAL")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.BLL")).Where(type => type.Name.EndsWith("BLL")).AsImplementedInterfaces();

            /*AsImplementedInterfaces() 可以使类自动找到它所有继承的接口*/

            //批量注册依赖到自己，这样可以在使用类进行注入
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.DAL")).AsSelf();
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.BLL")).AsSelf();

            //当个设置类下的属性注册
            //builder.RegisterType<Controllers.HomeController>().AsSelf().PropertiesAutowired().InstancePerDependency();

            //批量注册属性注入
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.BLL"))
               .AsImplementedInterfaces()
               .Where(t => t.Name.EndsWith("DAL"))
               .PropertiesAutowired() //设置注册时自动把属性也注入
               .AsSelf()
               .InstancePerDependency();

            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.DAL"))
             .AsImplementedInterfaces()
             .Where(t => t.Name.EndsWith("BLL"))
             .PropertiesAutowired() //设置注册时自动把属性也注入
             .AsSelf()
             .InstancePerDependency();

            //设置Colltroller属性注入
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
              .AsImplementedInterfaces()
              .Where(t => t.Name.EndsWith("Controller"))
              .PropertiesAutowired() //设置注册时自动把属性也注入
              .AsSelf()
              .InstancePerDependency();


            /*多个类继承同一个接口的时候，后面的注册的依赖关系会覆盖点前面注册的依赖关系，如果不想覆盖可以使用PreserveExistingDefaults方法
                builder.RegisterType<DogRepository>().As<IAnimalRepository>();
                builder.RegisterType<CatRepository>().As<IAnimalRepository>();
                builder.RegisterType<CatRepository>().As<IAnimalRepository>().PreserveExistingDefaults(); //实际注册的是DogRepository
            */
            /*多个类实现同一个接口起名,使用时要加上Name
             builder.RegisterType<DogRepository>().Named<IAnimalRepository>("Dog_IAnimal");
             builder.RegisterType<CatRepository>().Named<IAnimalRepository>("Cat_IAnimal");
             var dbRepository = container.ResolveNamed<IAnimalRepository>("Cat_IAnimal");  
             */

            /*定义生命周期
              builder.RegisterType<Worker>().InstancePerDependency()：(瞬时)用于控制对象的生命周期，每次加载实例时都是新建一个实例，默认就是这种方式
              builder.RegisterType<Worker>().SingleInstance()：（单例）用于控制对象的生命周期，每次加载实例时都是返回同一个实例
             */

            /*
             * .Net Framwork注册控制器下的属性，注意属性要是public的
             * builder_mvc.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired().InstancePerDependency();
             */

            //通过注册模块来注入(重要：可以把上面注入的代码都移到一个类里面，然后把该类以模块的方式注入)
            //builder.RegisterModule<CustomAutofacModule>();
        }
    }

    //public class CustomAutofacModule : Module
    //{
    //    protected override void Load(ContainerBuilder builder)
    //    {
    //        //base.Load(builder);

    //        var containerBuilder = new ContainerBuilder();
    //        containerBuilder.RegisterType<SmsDAL>().As<IMessageDAL>(); 
    //    }
    //}


}
