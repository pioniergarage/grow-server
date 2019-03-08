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
    public abstract class BasePublicController : Controller
    {
        protected GrowDbContext DbContext { get; }

        protected AppSettings AppSettings { get; }
        
        protected string CurrentContestYear { get; private set; }

        protected string CurrentContestName { get; private set; }


        protected Contest CurrentContest => DbContext.Contests
            .Single(c => c.Year == CurrentContestYear);

        protected IQueryable<Event> CurrentEvents => DbContext.Events
            .Where(e => e.Contest.Year == CurrentContestYear);

        protected IQueryable<Partner> CurrentPartners => DbContext.Partners
            .Where(p => DbContext.Contests.Any(c => c.Year == CurrentContestYear && c.Partners.Any(l => l.Partner.Equals(p))));

        protected IQueryable<Person> CurrentMentors => DbContext.Persons
            .Where(p => DbContext.Contests.Any(c => c.Year == CurrentContestYear && c.Mentors.Any(l => l.Person.Equals(p))));

        protected IQueryable<Person> CurrentOrganizers => DbContext.Persons
            .Where(p => DbContext.Contests.Any(c => c.Year == CurrentContestYear && c.Organizers.Any(l => l.Person.Equals(p))));

        protected IQueryable<Person> CurrentJudges => DbContext.Persons
            .Where(p => DbContext.Contests.Any(c => c.Year == CurrentContestYear && c.Judges.Any(l => l.Person.Equals(p))));

        protected IQueryable<Prize> CurrentPrizes => DbContext.Prizes
            .Where(e => e.Contest.Year == CurrentContestYear);

        protected IQueryable<Team> CurrentTeams => DbContext.Teams
            .Where(e => e.Contest.Year == CurrentContestYear);


        protected BasePublicController(GrowDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            DbContext = dbContext;
            AppSettings = appSettings.Value;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CurrentContestYear = AppSettings.CurrentContestYear;
            if (context.RouteData.Values.TryGetValue("year", out object value) && value is string year)
            {
                CurrentContestYear = year;
            }
            CurrentContestName = DbContext.Contests.Where(c => c.Year == CurrentContestYear).Select(c => c.Name).Single();

            // Default values for all controller actions
            ViewBag.TransparentNavbar = false;
            ViewBag.CurrentContestName = CurrentContestName;

            base.OnActionExecuting(context);
        }
    }
}