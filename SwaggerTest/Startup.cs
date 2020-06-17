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

                //��Ŀ��������=������=���ѡ�XML �ļ�·���� ���ϣ�����Ϊ��bin\Debug\netcoreapp3.1\SwaggerTest.xml��
                //string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"bin\\Debug\\netcoreapp3.1\\DotnetCore.xml");
                string path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "SwaggerTest.xml");//

                option.IncludeXmlComments(path);
            });

            //��������������е�������Ҫaddһ�£�useһ�£�Ȼ������ڿ������򷽷��ϴ�EnableCors("any")��ǩ�����Ҫ���ÿ���ʹ��DisableCors����
            services.AddCors(m =>
            {
                //��Ӳ���
                m.AddPolicy("Any", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                m.AddPolicy("SomeWebSite", a =>
                {
                    a.WithOrigins("http://www.gongkongbpo.com", "http://www.gongkongbpo1.com", "http://www.gongkongbpo2.com");
                });
            });

            //�����û��棬ResponseCache���ֱ��������������棻
            //���add,use һ�¾ͻ��ɷ���˵Ļ��� 
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

            //�������use 
            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                //һ��С�ӣ���һ�������е�v1Ҫ�������ConfigureServices������v1һ��
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "swaggerTest");
            });


            //����
            app.UseCors();

            //����˻���
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
