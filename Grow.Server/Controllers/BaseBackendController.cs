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

namespace Grow.Server.Controllers
{
    public abstract class BaseBackendController : BaseController
    {
        public bool IsLoggedIn => User?.Identity.IsAuthenticated ?? false;

        public Claim MyTeamClaim
        {
            get
            {
                return _teamClaim
                    ?? (_teamClaim = User?.Claims?.FirstOrDefault(c => c.Type.Equals(Constants.TEAM_CLAIM_TYPE)));
            }
        }
        private Claim _teamClaim;

        public int MyTeamId
        {
            get
            {
                return _teamId
                    ?? (_teamId = int.Parse(MyTeamClaim?.Value ?? "0")) ?? 0;
            }
        }
        private int? _teamId;

        public IQueryable<Team> MyTeamQuery
        {
            get
            {
                return _teamQuery
                    ?? (_teamQuery = DbContext.Teams.Where(t => t.Id == MyTeamId));
            }
        }
        private IQueryable<Team> _teamQuery;

        protected BaseBackendController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Default values for all controller actions
            ViewBag.TeamId = MyTeamId;
            ViewBag.IsLoggedIn = IsLoggedIn;
            ViewBag.IsAdmin = IsLoggedIn && User.IsInRole(Constants.ADMIN_ROLE_NAME);
            ViewBag.IsSuperAdmin = IsLoggedIn && User.IsInRole(Constants.SUPERADMIN_ROLE_NAME);
        }
    }
}
