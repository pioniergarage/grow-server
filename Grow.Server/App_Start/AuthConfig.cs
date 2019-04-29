using Grow.Server.Model;
using Grow.Server.Model.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.App_Start
{
    internal static class AuthConfig
    {
        public static void AddGrowAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<Account, IdentityRole>()
                .AddEntityFrameworkStores<GrowDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromDays(14);
            });

            services.AddTransient<IAuthDataInitializer, AuthDataInitializer>();
        }

        public static void UseGrowAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            var seederTask = AuthDataInitializer.Run(app.ApplicationServices);
            seederTask.Wait();
        }
    }

    internal interface IAuthDataInitializer
    {
        Task InitializeAsync();
    }

    internal class AuthDataInitializer : IAuthDataInitializer
    {
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        private const string _adminRoleName = Constants.ADMIN_ROLE_NAME;
        private readonly string[] _defaultRoles = new string[] { _adminRoleName, Constants.TEAM_ROLE_NAME };

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Account> _userManager;

        public static async Task Run(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var instance = serviceScope.ServiceProvider.GetRequiredService<IAuthDataInitializer>();
                await instance.InitializeAsync().ConfigureAwait(false);
            }
        }

        public AuthDataInitializer(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _adminEmail = configuration.GetSection("AppSettings").GetValue<string>(Constants.ADMIN_EMAIL_SETTING);
            _adminPassword= configuration.GetSection("AppSettings").GetValue<string>(Constants.ADMIN_PASSWORD_SETTING);
        }

        public async Task InitializeAsync()
        {
            await EnsureRolesAsync().ConfigureAwait(false);
            await EnsureDefaultAdminAsync().ConfigureAwait(false);
        }

        protected async Task EnsureRolesAsync()
        {
            foreach (var role in _defaultRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role).ConfigureAwait(false))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role)).ConfigureAwait(false);
                }
            }
        }

        protected async Task EnsureDefaultAdminAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(_adminRoleName).ConfigureAwait(false);

            if (!adminUsers.Any())
            {
                var adminUser = new Account()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = _adminEmail,
                    UserName = _adminEmail,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                var result = await _userManager.CreateAsync(adminUser, _adminPassword).ConfigureAwait(false);
                await _userManager.AddToRoleAsync(adminUser, _adminRoleName).ConfigureAwait(false);
            }
        }
    }
}
