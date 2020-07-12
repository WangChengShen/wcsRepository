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
        ///  ����������Development,��������� Configure+��������+Services�ķ����󣬾ͻḲ�ǵ�ConfigureServices�ķ���
        /// </summary>
        /// <param name="services"></param>
        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    //������ȡĳһ�������ļ�
        //    string name = Configuration.GetValue<string>("Name");
        //    string name2 = Configuration["Name"];
        //    string Default = Configuration.GetValue<string>("Logging:LogLevel:Default");

        //    //ȫ���󶨣��������ļ������󶨵�һ��������
        //    var appSetting = new AppSetting();
        //    Configuration.Bind(appSetting);

        //    //���ְ�
        //    var logging = new Logging();
        //    Configuration.GetSection("Logging").Bind(logging);

        //    //ע��Ϊ������Controller����ʹ��ʱע�� IOptions<AppSetting> appSetting
        //    services.Configure<AppSetting>(Configuration);

        //    //����Զ���������ļ�������ȡ����ȡʱAddJsonFile����������д·��
        //    var myConfig = new ConfigurationBuilder().AddJsonFile("mySetting.json").Build();
        //    //ע��Ϊ����
        //    services.Configure<Logging>(myConfig);

        //    services.AddControllersWithViews();
        //}


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region �����ļ��Ķ�ȡ 
            //������ȡĳһ�������ļ�
            string name = Configuration.GetValue<string>("Name");
            string name2 = Configuration["Name"];
            string Default = Configuration.GetValue<string>("Logging:LogLevel:Default");

            //ȫ���󶨣��������ļ������󶨵�һ��������
            var appSetting = new AppSetting();
            Configuration.Bind(appSetting);

            //���ְ�
            var logging = new Logging();
            Configuration.GetSection("Logging").Bind(logging);

            //ע��Ϊ������Controller����ʹ��ʱע�� IOptions<AppSetting> appSetting
            services.Configure<AppSetting>(Configuration);

            //����Զ���������ļ�������ȡ����ȡʱAddJsonFile����������д·��
            var myConfig = new ConfigurationBuilder().AddJsonFile("mySetting.json").Build();
            //ע��Ϊ����
            services.Configure<Logging>(myConfig);
            #endregion

            #region Core���õ�����ע�� ����һ���ľ����ԣ���֧�ֹ��캯��ע�룩
            //Core���õ�����ע��,ͬһ���ӿ�ע����ʵ�ֵĻ�������ĻḲ��ǰ���
            services.AddTransient<IMessageService, SmsService>();//˲ʱģʽ��ÿ�ε��ö���newһ��ʵ��
            services.AddScoped<IMessageService, SmsService>();//������ģʽ(����������ÿ������ʹ��һ��ʵ��)��ÿ��http����ʼ��newһ��ʵ���������������´�ʹ����ֱ��������
            services.AddSingleton<IMessageService, SmsService>();//����ģʽ������Ӧ�ö�����ͬһ��ʵ��
            #endregion

            #region core���Զ������һ������д���Ƚ����� 
            //core���Զ������һ������д���Ƚ�����
            services.AddMessage();//û�����ã�ʹ��Ĭ�ϵ�
            services.AddMessage(builder => builder.SendEmail()); //�������õ�
            #endregion

            #region Session��ʹ��
            /*Core����sessionҲ�Ƕ��Ƶ�,ʹ��ʱҪ��Service����AddSession,
             Ȼ����Configure�������UserSession
             */
            services.AddSession();
            #endregion

            //web��Ŀ�������service
            services.AddControllersWithViews(); 
        }

        /// <summary>
        /// ����������Development,��������� Configure+���������ķ����󣬾ͻḲ�ǵ�Configure����
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
            app.UseStaticFiles(new StaticFileOptions()  //ָ����̬�ļ��ĵ�ַ������������йܵ�ʱ��Ҫִ���£���Ȼ�Ҳ�����ʽ
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot")),
                //OnPrepareResponse = c =>
                //{
                //    c.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "no-cache";
                //}
            });

            //ʹ��session
            app.UseSession();
            //�Զ����м��
            // app.UseMiddleware<TestMiddleWare>();
            //���������ٰ�һ�㣬����չ����
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


            //�ս���м��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
