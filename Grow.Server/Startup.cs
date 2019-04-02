using Grow.Server.Model;
using Grow.Server.Model.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Grow.Server
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            // DB setup
            services.AddDbContext<GrowDbContext>(
                options => options
                    .EnableSensitiveDataLogging(true)
                    .UseSqlServer(
                        Configuration.GetConnectionString("GrowDbContext")
                    )
            );

            // App settings
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Setup MVC pipeline
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "year-selection default",
                    template: "{year}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index", year = "" },
                    constraints: new { year = @"\d{4}" }
                );

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "current year default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index", year = "" }
                );
            });
        }
    }
}
