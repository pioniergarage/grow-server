using System;
using System.Linq;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Controllers;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
    public abstract class BaseAdminController : BaseBackendController
    {
        protected BaseAdminController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }
        
        protected void RemoveNavigationPropertiesFromModelState<T>()
        {
            var navProperties = typeof(T)
                .GetProperties()
                .Where(p => p.GetMethod.IsVirtual && typeof(BaseEntity).IsAssignableFrom(p.PropertyType));
            foreach (var property in navProperties)
            {
                var navKeys = ModelState.Keys.Where(k => k.StartsWith(property.Name + "."));
                foreach (var key in navKeys)
                    ModelState.Remove(key);
            }
        }
    }
}