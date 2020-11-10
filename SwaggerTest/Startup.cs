using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wcs.BLL;
using Wcs.DAL;

namespace SwaggerTest
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
             1.nuget引入 Swashbuckle.AspNetCore
             2.在ConfigureServices 和Configure 都加下
             3.另一个知识点，设置项目启动页面，在launchSettings.json配置文件里面配置launchUrl节点
             */
            services.AddSwaggerGen(option =>
            {
                /*swagger 分组功能：
                 * 1.在这里创建多个SwaggerDoc，name是分组的名称
                 * 2.在Config里面创建多个SwaggerEndpoint，
                 * 3.给控制器或则方法打上    [ApiExplorerSettings(GroupName = "gkBpoApi")]
                 */

                option.SwaggerDoc("group1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "wcs接口api1",
                    Version = "version1" //这个是自己定的
                });
                option.SwaggerDoc("group2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "wcs接口api2",
                    Version = "version1" //这个是自己定的
                });

                //项目属性里面=》生成=》把“XML 文件路径” 勾上，设置为“bin\Debug\netcoreapp3.1\SwaggerTest.xml”
                //string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"bin\\Debug\\netcoreapp3.1\\DotnetCore.xml");
                string path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "SwaggerTest.xml");//

                option.IncludeXmlComments(path);
            });

            //设置允许跨域，所有的请求，需要add一下，use一下，然后可以在控制器或方法上打EnableCors("any")标签，如果要禁用跨域使用DisableCors特性
            services.AddCors(m =>
            {
                //添加策略
                m.AddPolicy("Any", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                m.AddPolicy("SomeWebSite", a =>
                {
                    a.WithOrigins("http://www.gongkongbpo.com", "http://www.gongkongbpo1.com", "http://www.gongkongbpo2.com");
                });
            });

            //简单设置缓存，ResponseCache如果直接用是浏览器缓存；
            //如果add,use 一下就会变成服务端的缓存 
            services.AddResponseCaching();

            services.AddTransient<IStudentDAL, StudentDAL>();
            services.AddTransient<IStudentBLL, StudentBLL>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            //添加两个use 
            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                //一个小坑，第一个参数中的v1要和上面的ConfigureServices方法的v1一样
                option.SwaggerEndpoint("/swagger/group1/swagger.json", "group1");
                option.SwaggerEndpoint("/swagger/group2/swagger.json", "group2");
            });
             
            //跨域
            app.UseCors();

            //服务端缓存
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
