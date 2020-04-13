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
             1.nuget���� Swashbuckle.AspNetCore
             2.��ConfigureServices ��Configure ������
             3.��һ��֪ʶ�㣬������Ŀ����ҳ�棬��launchSettings.json�����ļ���������launchUrl�ڵ�
             */
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SwaggerTest",
                    Version = "version1" //������Լ�����
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

            //�������use 
            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                //һ��С�ӣ���һ�������е�v1Ҫ�������ConfigureServices������v1һ��
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "swaggerTest");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
