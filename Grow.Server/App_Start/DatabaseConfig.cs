using Grow.Data;
using Grow.Server.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grow.Server.App_Start
{
    internal static class DatabaseConfig
    {
        public static void AddGrowDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GrowDbContext>(
                options => options
                    .EnableSensitiveDataLogging(true)
                    .UseSqlServer(
                        configuration.GetConnectionString("GrowDbContext")
                    )
            );
        }

        public static void UpdateGrowDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<GrowDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
