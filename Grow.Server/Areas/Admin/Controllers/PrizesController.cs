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
    public class PrizesController : BaseEntityAdminController<Prize>
    {
        public PrizesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }
        
        protected override void AddEntityListsToViewBag()
        {
            ViewBag.Types = ViewHelpers.SelectListFromEnum<Prize.PrizeType>();
        }
        
        protected override IQueryable<Prize> IncludeNavigationProperties(IQueryable<Prize> query)
        {
            return query
                .Include(t => t.Contest)
                .Include(t => t.GivenBy)
                .Include(t => t.Winner);
        }

        protected override Expression<Func<Contest, ICollection<Prize>>> EntitiesInContestExpression()
        {
            return c => c.Prizes;
        }
    }
}
