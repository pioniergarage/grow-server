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

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Team")]
    [Authorize(Policy = Constants.TEAM_CLAIM_POLICY_NAME)]
    public abstract class BaseTeamController : BaseController
    {
        public Claim TeamClaim { get; }
        public int TeamId { get; }

        protected BaseTeamController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
            TeamClaim = User.Claims.FirstOrDefault(c => c.Type.Equals(Constants.TEAM_CLAIM_TYPE));
            TeamId = int.Parse(TeamClaim?.Value ?? "0");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Default values for all controller actions
            ViewBag.TeamId = TeamId;
        }
    }
}