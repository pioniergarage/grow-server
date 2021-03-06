﻿using System.Linq;
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
    public class TeamsController : BaseEntityAdminController<Team>
    {
        public TeamsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        protected override void AddEntityListsToViewBag()
        {
        }

        protected override Expression<Func<Contest, ICollection<Team>>> EntitiesInContestExpression()
        {
            return c => c.Teams;
        }

        protected override IQueryable<Team> IncludeNavigationProperties(IQueryable<Team> query)
        {
            return query
                .Include(t => t.Contest)
                .Include(t => t.LogoImage)
                .Include(t => t.TeamPhoto);
        }
    }
}
