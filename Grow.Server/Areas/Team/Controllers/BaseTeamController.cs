using System;
using System.Linq;
using System.Security.Claims;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Controllers;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.Team.Controllers
{
    [Area("Team")]
    [Authorize(Policy = Constants.TEAM_CLAIM_POLICY_NAME)]
    public abstract class BaseTeamController : BaseBackendController
    {
        protected BaseTeamController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (MyTeamQuery.Any() == false)
                throw new UnauthorizedAccessException("User has no access to a team");

            base.OnActionExecuting(context);
        }
    }
}