using System;
using System.Linq;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Controllers;
using Grow.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
    public abstract class BaseAdminController : BaseController
    {
        protected BaseAdminController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
            GlobalFilter = _ => true; // show all entities in admin area
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Default values for all controller actions
            ViewBag.AllContests = DbContext.Contests.AsNoTracking().ToList();
        }
    }
}