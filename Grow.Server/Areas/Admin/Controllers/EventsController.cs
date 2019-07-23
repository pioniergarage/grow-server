using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Grow.Server.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Grow.Server.Areas.Admin.Controllers
{
    public class EventsController : BaseEntityAdminController<Event>
    {
        public EventsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) 
            : base(dbContext, appSettings, logger)
        {
        }

        protected override void AddEntityListsToViewBag()
        {
            ViewBag.Visibilities = ViewHelpers.SelectListFromEnum<Event.EventVisibility>();
            ViewBag.Types = ViewHelpers.SelectListFromEnum<Event.EventType>();
        }

        protected override Expression<Func<Contest, ICollection<Event>>> EntitiesInContestExpression()
        {
            return c => c.Events;
        }

        protected override IQueryable<Event> IncludeNavigationProperties(IQueryable<Event> query)
        {
            return query
                .Include(t => t.Contest)
                .Include(t => t.Image)
                .Include(t => t.HeldBy)
                .Include(t => t.Responses);
        }
    }
}
