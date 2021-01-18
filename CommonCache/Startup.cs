using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonCache.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;

namespace CommonCache
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
            services.AddControllersWithViews();

            //ʹ��MemoryCacheʱ�������������������Ӧ�Ĳ�����Ҳ���Բ����ã�ʹ��Ĭ�ϵ�����
            services.AddMemoryCache(c =>
            {
                c.Clock = new LocalClock();
                //c.SizeLimit = 1000; //ָ�����
            });

            //����add,user�������ResponseCache�ͱ�Ϊ�˷���˻���
            //services.AddResponseCaching(options =>
            //{
            //    options.UseCaseSensitivePaths = false;
            //    options.MaximumBodySize = 1024;
            //    options.SizeLimit = 100 * 1024 * 1024;
            //});

            #region redis�ֲ�ʽ���� DistributedRedisCache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "RedisDistributedCache1";
            });
            #endregion

        }

        /// <summary>
        /// ָ������ʱ�䣬�ñ���ʱ��
        /// </summary>
        public class LocalClock : ISystemClock
        {
            public DateTimeOffset UtcNow => DateTime.Now;
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
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                //OnPrepareResponse = c =>
                //{
                //    c.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "no-cache";
                //}
            });

            app.UseRouting();

            app.UseAuthorization();

            //�����Զ�����м��
            //app.UseMiddleware<ExceptionHandleMiddleware>();
            //���߷�װΪ��չ����
            app.UseExceptionHandle();

            //ResponseCaching
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
