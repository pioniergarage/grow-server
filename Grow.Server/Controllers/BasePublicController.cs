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
    public abstract class BasePublicController : BaseController
    {
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

        protected BasePublicController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Default values for all controller actions
            ViewBag.TransparentNavbar = false;
            ViewBag.CurrentContestName = CurrentContestName;
        }
    }
}