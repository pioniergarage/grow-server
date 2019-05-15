using System.Linq;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Grow.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Grow.Server.Controllers
{
    public abstract class BaseController : Controller
    {
        protected GrowDbContext DbContext { get; }

        protected AppSettings AppSettings { get; }

        protected string SelectedContestYear { get; private set; }

        protected string SelectedContestName { get; private set; }

        protected Func<BaseEntity, bool> GlobalFilter = (e => e.IsActive); // hide inactive entities

        protected IQueryable<Contest> SelectedContest => DbContext.Contests
            .Where(c => c.Year == SelectedContestYear && GlobalFilter(c));

        protected IQueryable<Event> EventsInSelectedYear => DbContext.Events
            .Where(e => e.Contest.Year == SelectedContestYear && GlobalFilter(e));

        protected IQueryable<Partner> PartnersInSelectedYear => DbContext.Partners
            .Where(p => p.Contest.Year == SelectedContestYear && GlobalFilter(p));

        protected IQueryable<Mentor> MentorsInSelectedYear => DbContext.Mentors
            .Where(m => m.Contest.Year == SelectedContestYear && GlobalFilter(m));

        protected IQueryable<Organizer> OrganizersInSelectedYear => DbContext.Organizers
            .Where(o => o.Contest.Year == SelectedContestYear && GlobalFilter(o));

        protected IQueryable<Judge> JudgesInSelectedYear => DbContext.Judges
            .Where(j => j.Contest.Year == SelectedContestYear && GlobalFilter(j));

        protected IQueryable<Prize> PrizesInSelectedYear => DbContext.Prizes
            .Where(p => p.Contest.Year == SelectedContestYear && GlobalFilter(p));

        protected IQueryable<Team> TeamsInSelectedYear => DbContext.Teams
            .Where(t => t.Contest.Year == SelectedContestYear && GlobalFilter(t));

        protected BaseController(GrowDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            DbContext = dbContext;
            AppSettings = appSettings.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ChooseSelectedContestYear(context);

            // Default values for all controller actions
            ViewBag.SelectedContestYear = SelectedContestYear;
            ViewBag.SelectedContestName = SelectedContestName;
        }

        public void ChooseSelectedContestYear(ActionExecutingContext context)
        {
            // STEP 1: Choose year
            // fallback: use default defined in AppSettings
            SelectedContestYear = AppSettings.CurrentContestYear;
            // Contest chosen via route (/year/controller/action)
            if (context.RouteData.Values.TryGetValue(Constants.ROUTE_YEAR_SELECTOR, out object value) && value is string year && !string.IsNullOrEmpty(year))
            {
                SelectedContestYear = year;
            }
            // Contest chosen via cookie 
            else if (!string.IsNullOrEmpty(year = context.HttpContext.Request.Cookies[Constants.COOKIE_SELECTED_YEAR_KEY]))
            {
                SelectedContestYear = year;
            }

            // STEP 2: Check year
            // pull selected year from db
            var selectedContest = DbContext.Contests.AsNoTracking().Where(c => c.Year == SelectedContestYear).SingleOrDefault();
            if (selectedContest == null)
            {
                context.Result = new StatusCodeResult(404);
                return;
            }
            // check access to contest
            var isContextPublic = selectedContest.IsActive;
            var isUserAdmin = User.Identity.IsAuthenticated && User.IsInRole(Constants.ADMIN_ROLE_NAME);
            if (isContextPublic || isUserAdmin)
            {
                // all good!
                SelectedContestName = selectedContest.Name;
            }
            else if (SelectedContestName == null)
            {
                context.Result = new StatusCodeResult(404);
            }
        }
    }
}