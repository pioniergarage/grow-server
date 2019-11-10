using Grow.Data;
using Grow.Server.App_Start;
using Grow.Server.Model;
using Grow.Server.Model.Config;
using Grow.Server.Model.Extensions;
using Grow.Server.Model.Helpers;
using Grow.Server.Model.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

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
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new IgnoreNavigationsJsonResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Singletons
            services.AddSingleton<ILogger, Logger>();

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
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            // Culture
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");

            // DB setup
            app.UpdateGrowDatabase();
            
            // Setup MVC pipeline
            app.UseLoggingExceptionHandler();
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithRedirects("/ErrorCode?code={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseGrowAuthentication();
            app.UseMvc(RouteConfig.GetGrowRoutes());
        }
    }
}
