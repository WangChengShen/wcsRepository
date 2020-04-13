using System;
using System.Collections.Generic;
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
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SwaggerTest",
                    Version = "version1" //这个是自己定的
                });
            });

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
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "swaggerTest");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
