using System;
using System.Collections.Generic;
using System.Linq;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Grow.Server.Areas.Admin.Controllers.Api
{
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
    public abstract class ApiController<T> : ControllerBase where T : BaseTimestampedEntity
    {
        public GrowDbContext Context { get; }
        public ILogger Logger { get; }

        protected ApiController(GrowDbContext context, ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        [HttpGet]
        public virtual ActionResult<IEnumerable<T>> Find(string search = null, string year = null)
        {
            IQueryable<T> query = Context.Set<T>();
            if (search != null)
                query = query.Where(e => e.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
            if (year != null && typeof(BaseContestSubEntity).IsAssignableFrom(typeof(T)))
            {
                var contest = Context.Contests.SingleOrDefault(c => c.Year.Equals(year));
                if (contest != null)
                    query = query.Where(e => (e as BaseContestSubEntity).ContestId == contest.Id);
            }
            return Ok(query);
        }

        [HttpGet("{id}")]
        public ActionResult<T> Get(int id)
        {
            var entity = Context.Set<T>().Find(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }
    }

    public class ContestsController : ApiController<Contest>
    {
        public ContestsController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class EventsController : ApiController<Event>
    {
        public EventsController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class JudgesController : ApiController<Judge>
    {
        public JudgesController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class MentorsController : ApiController<Mentor>
    {
        public MentorsController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class OrganizersController : ApiController<Organizer>
    {
        public OrganizersController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class PartnersController : ApiController<Partner>
    {
        public PartnersController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class PrizesController : ApiController<Prize>
    {
        public PrizesController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }

    public class TeamsController : ApiController<Team>
    {
        public TeamsController(GrowDbContext context, ILogger logger) : base(context, logger) { }
    }
}
