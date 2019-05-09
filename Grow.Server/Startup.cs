using Grow.Data;
using Grow.Server.App_Start;
using Grow.Server.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddGrowDatabase(Configuration);

            // Auth setup
            services.AddGrowAuthentication();

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

            // DB setup
            app.UpdateGrowDatabase();

            // Setup MVC pipeline
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseGrowAuthentication();
            app.UseMvc(RouteConfig.GetGrowRoutes());
        }
    }
}
