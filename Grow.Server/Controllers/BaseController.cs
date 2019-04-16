using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
using Grow.Server.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Grow.Server.Controllers
{
    public abstract class BaseController : Controller
    {
        protected GrowDbContext DbContext { get; }

        protected AppSettings AppSettings { get; }

        protected string CurrentContestYear { get; private set; }

        protected string CurrentContestName { get; private set; }

        protected Contest CurrentContest => DbContext.Contests
            .Single(c => c.Year == CurrentContestYear);

        protected BaseController(GrowDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            DbContext = dbContext;
            AppSettings = appSettings.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CurrentContestYear = AppSettings.CurrentContestYear;
            if (context.RouteData.Values.TryGetValue("year", out object value) && value is string year && !string.IsNullOrEmpty(year))
            {
                CurrentContestYear = year;
            }
            CurrentContestName = DbContext.Contests.Where(c => c.Year == CurrentContestYear).Select(c => c.Name).SingleOrDefault();
            if (CurrentContestName == null)
            {
                context.Result = new StatusCodeResult(404);
            }
        }
    }
}