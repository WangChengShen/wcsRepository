using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wcs.Models;
using Wcs.Models.Interface;

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
            services.AddTransient<IMessageService, SmsService>();//˲ʱģʽ��ÿ�ε��ö���newһ��ʵ��
            services.AddScoped<IMessageService, SmsService>();//������ģʽ(����������ÿ������ʹ��һ��ʵ��)��ÿ��http����ʼ��newһ��ʵ���������������´�ʹ����ֱ��������
            services.AddSingleton<IMessageService, SmsService>();//����ģʽ������Ӧ�ö�����ͬһ��ʵ��
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
            builder.RegisterType<SmsService>().As<IMessageService>();

            //builder.RegisterModule<CustomAutofacModule>();
        }
    }

    //public class CustomAutofacModule : Module
    //{
    //    protected override void Load(ContainerBuilder builder)
    //    {
    //        //base.Load(builder);

    //        var containerBuilder = new ContainerBuilder();
    //        containerBuilder.RegisterType<SmsService>().As<IMessageService>();
             
    //    }
    //}


}
