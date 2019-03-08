using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
using Grow.Server.Model.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Grow.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .PrepareDatabase()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public static class ProgramExtensions
    {
        public static IWebHost PrepareDatabase(this IWebHost webHost)
        {
            var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

            // Run migration and seeding
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<GrowDbContext>();
                context.ResetDatabase();
                context.SeedDataFrom2018();
                context.SeedDataFrom2017();
            }

            return webHost;
        }
    }
}
