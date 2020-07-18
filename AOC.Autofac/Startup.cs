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
                AOC��DI������ע�룩������
                AOC����������Ҫ��ɵ�Ŀ�꣬���������ŵ㣺
                1.���2������ϸ��
                DI������ע�룬��һ���ֶΣ����繹��ʵ��Bʱ����Ҫʵ��A������Զ�����A���뵽B�Ĺ��캯��������C��ҪB������Զ�����B;
                ���ݵݹ�ķ�ʽ�������޹��죻

            Core�������õ������͵�������ע����������ͬʱʹ�õģ�
             */
            #region Core���õ�����ע�� ����һ���ľ����ԣ���֧�ֹ��캯��ע�룩
            //Core���õ�����ע��,ͬһ���ӿ�ע����ʵ�ֵĻ�������ĻḲ��ǰ���
            //services.AddTransient<IMessageService, SmsService>();//˲ʱģʽ��ÿ�ε��ö���newһ��ʵ��
            //services.AddScoped<IMessageService, SmsService>();//������ģʽ(����������ÿ������ʹ��һ��ʵ��)��ÿ��http����ʼ��newһ��ʵ���������������´�ʹ����ֱ��������
            //services.AddSingleton<IMessageService, SmsService>();//����ģʽ������Ӧ�ö�����ͬһ��ʵ��
            #endregion

            #region Autofac��ʹ��
            /*��ΪCore���õ�����ע����һ���ľ����ԣ�����һ��������������IOC������
             Autofac��ʹ�ã���autofac�Ĺ�����������ҵ�ʹ�÷��� https://autofac.readthedocs.io/en/latest/integration/aspnetcore.html����
             1.nuget����������:Autofac��Autofac.Extensions.DependencyInjection  (Autofac.Extras.DynamicProxy����������������autofac��aop�Ĺ���)
             2.��Program����ָ������ʹ��autofac
             3.���ConfigureContainer��������������������д������ʵ��ӳ��
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
            //����ע��
            builder.RegisterType<SmsDAL>().As<IMessageDAL>();
            //builder.RegisterType<SmsDAL>().AsImplementedInterfaces();//�Զ��Ҽ̳нӿ�

            //����ע���������ӿ�
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.DAL")).Where(type => type.Name.EndsWith("DAL")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.BLL")).Where(type => type.Name.EndsWith("BLL")).AsImplementedInterfaces();

            /*AsImplementedInterfaces() ����ʹ���Զ��ҵ������м̳еĽӿ�*/

            //����ע���������Լ�������������ʹ�������ע��
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.DAL")).AsSelf();
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.BLL")).AsSelf();

            //�����������µ�����ע��
            //builder.RegisterType<Controllers.HomeController>().AsSelf().PropertiesAutowired().InstancePerDependency();

            //����ע������ע��
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.BLL"))
               .AsImplementedInterfaces()
               .Where(t => t.Name.EndsWith("DAL"))
               .PropertiesAutowired() //����ע��ʱ�Զ�������Ҳע��
               .AsSelf()
               .InstancePerDependency();

            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Wcs.DAL"))
             .AsImplementedInterfaces()
             .Where(t => t.Name.EndsWith("BLL"))
             .PropertiesAutowired() //����ע��ʱ�Զ�������Ҳע��
             .AsSelf()
             .InstancePerDependency();

            //����Colltroller����ע��
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
              .AsImplementedInterfaces()
              .Where(t => t.Name.EndsWith("Controller"))
              .PropertiesAutowired() //����ע��ʱ�Զ�������Ҳע��
              .AsSelf()
              .InstancePerDependency();


            /*�����̳�ͬһ���ӿڵ�ʱ�򣬺����ע���������ϵ�Ḳ�ǵ�ǰ��ע���������ϵ��������븲�ǿ���ʹ��PreserveExistingDefaults����
                builder.RegisterType<DogRepository>().As<IAnimalRepository>();
                builder.RegisterType<CatRepository>().As<IAnimalRepository>();
                builder.RegisterType<CatRepository>().As<IAnimalRepository>().PreserveExistingDefaults(); //ʵ��ע�����DogRepository
            */
            /*�����ʵ��ͬһ���ӿ�����,ʹ��ʱҪ����Name
             builder.RegisterType<DogRepository>().Named<IAnimalRepository>("Dog_IAnimal");
             builder.RegisterType<CatRepository>().Named<IAnimalRepository>("Cat_IAnimal");
             var dbRepository = container.ResolveNamed<IAnimalRepository>("Cat_IAnimal");  
             */

            /*������������
              builder.RegisterType<Worker>().InstancePerDependency()��(˲ʱ)���ڿ��ƶ�����������ڣ�ÿ�μ���ʵ��ʱ�����½�һ��ʵ����Ĭ�Ͼ������ַ�ʽ
              builder.RegisterType<Worker>().SingleInstance()�������������ڿ��ƶ�����������ڣ�ÿ�μ���ʵ��ʱ���Ƿ���ͬһ��ʵ��
             */

            /*
             * .Net Framworkע��������µ����ԣ�ע������Ҫ��public��
             * builder_mvc.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired().InstancePerDependency();
             */

            //ͨ��ע��ģ����ע��(��Ҫ�����԰�����ע��Ĵ��붼�Ƶ�һ�������棬Ȼ��Ѹ�����ģ��ķ�ʽע��)
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
