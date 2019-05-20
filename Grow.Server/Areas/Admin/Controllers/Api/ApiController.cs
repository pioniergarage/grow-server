using System;
using System.Collections.Generic;
using System.Linq;
using Grow.Data;
using Grow.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Grow.Server.Areas.Admin.Controllers.Api
{
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class ApiController<T> : ControllerBase where T : BaseEntity
    {
        public GrowDbContext Context { get; }
        
        protected ApiController(GrowDbContext context)
        {
            Context = context;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<T>> Find(string search = null)
        {
            IQueryable<T> query = Context.Set<T>();
            if (search != null)
                query = query.Where(e => e.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
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

    public class ContestsController : ApiController<Contest> { public ContestsController(GrowDbContext context) : base(context) { } }
    public class EventsController : ApiController<Event> { public EventsController(GrowDbContext context) : base(context) { } }
    public class JudgesController: ApiController<Judge> { public JudgesController(GrowDbContext context) : base(context) { } }
    public class MentorsController: ApiController<Mentor> { public MentorsController(GrowDbContext context) : base(context) { } }
    public class OrganizersController : ApiController<Organizer> { public OrganizersController(GrowDbContext context) : base(context) { } }
    public class PartnersController : ApiController<Partner> { public PartnersController(GrowDbContext context) : base(context) { } }
    public class PrizesController : ApiController<Prize> { public PrizesController(GrowDbContext context) : base(context) { } }
    public class TeamsController : ApiController<Team> { public TeamsController(GrowDbContext context) : base(context) { } }
}
