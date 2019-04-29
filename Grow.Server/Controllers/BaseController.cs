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

        protected string SelectedContestYear { get; private set; }

        protected string SelectedContestName { get; private set; }

        protected IQueryable<Contest> SelectedContest => DbContext.Contests
            .Where(c => c.Year == SelectedContestYear);

        protected IQueryable<Event> EventsInSelectedYear => DbContext.Events
            .Where(e => e.Contest.Year == SelectedContestYear);

        protected IQueryable<Partner> PartnersInSelectedYear => DbContext.Partners
            .Where(p => DbContext.Contests.Any(c => c.Year == SelectedContestYear && c.Partners.Any(l => l.Partner.Equals(p))));

        protected IQueryable<Person> MentorsInSelectedYear => DbContext.Persons
            .Where(p => DbContext.Contests.Any(c => c.Year == SelectedContestYear && c.Mentors.Any(l => l.Person.Equals(p))));

        protected IQueryable<Person> OrganizersInSelectedYear => DbContext.Persons
            .Where(p => DbContext.Contests.Any(c => c.Year == SelectedContestYear && c.Organizers.Any(l => l.Person.Equals(p))));

        protected IQueryable<Person> JudgesInSelectedYear => DbContext.Persons
            .Where(p => DbContext.Contests.Any(c => c.Year == SelectedContestYear && c.Judges.Any(l => l.Person.Equals(p))));

        protected IQueryable<Prize> PrizesInSelectedYear => DbContext.Prizes
            .Where(e => e.Contest.Year == SelectedContestYear);

        protected IQueryable<Team> TeamsInSelectedYear => DbContext.Teams
            .Where(e => e.Contest.Year == SelectedContestYear);

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
            var selectedContest = DbContext.Contests.Where(c => c.Year == SelectedContestYear).SingleOrDefault();
            if (selectedContest == null)
            {
                context.Result = new StatusCodeResult(404);
                return;
            }
            // check access to contest
            var isContextPublic = true; //TODO: change once IsActive has been implemented
            var isUserAdmin = User.Identity.IsAuthenticated && User.IsInRole(Constants.ADMIN_ROLE_NAME);
            if (isContextPublic || isUserAdmin)
            {
                // all good!
                SelectedContestName = DbContext.Contests.Where(c => c.Year == SelectedContestYear).Select(c => c.Name).SingleOrDefault();
            }
            else if (SelectedContestName == null)
            {
                context.Result = new StatusCodeResult(404);
            }
        }
    }
}