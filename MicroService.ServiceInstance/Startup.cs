using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroService.ServiceInstance.unility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wcs.BLL;
using Wcs.DAL;

namespace MicroService.ServiceInstance
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
            services.AddControllers();

            services.AddTransient<IStudentDAL, StudentDAL>();
            services.AddTransient<IStudentBLL, StudentBLL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //用HTTPS重定向中间件, 这样就可以把所有的HTTP请求转换为HTTPS.
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //写在这个地方--进程启动后就会执行一次 且只执行一次,
            //先nuget引入Consul包，把服务注册进入Consul
            Configuration.ConsulRegist();
        }
    }
}
