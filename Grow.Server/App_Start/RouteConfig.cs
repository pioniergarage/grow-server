using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace Grow.Server.App_Start
{
    internal static class RouteConfig
    {
        public static Action<IRouteBuilder> GetGrowRoutes()
        {
            return routes =>
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
            };
        }
    }
}
