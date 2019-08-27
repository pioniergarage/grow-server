using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Grow.Server.Model.Helpers;
using System.Linq.Expressions;

namespace Grow.Server.Areas.Admin.Controllers
{
    public class QuestionsController : BaseEntityAdminController<CommonQuestion>
    {
        public QuestionsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) 
            : base(dbContext, appSettings, logger)
        {
        }

        protected override void AddEntityListsToViewBag()
        {
        }

        protected override Expression<Func<Contest, ICollection<CommonQuestion>>> EntitiesInContestExpression()
        {
            return c => c.CommonQuestions;
        }

        protected override IQueryable<CommonQuestion> IncludeNavigationProperties(IQueryable<CommonQuestion> query)
        {
            return query
                .Include(t => t.Contest);
        }
    }
}
