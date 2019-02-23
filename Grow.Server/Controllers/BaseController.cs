using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
using Grow.Server.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Grow.Server.Controllers
{
    public abstract class BaseController : Controller
    {
        protected GrowDbContext DbContext { get; set; }

        private int? _currentContestId;
        protected int CurrentContestId => _currentContestId ?? (_currentContestId = GetCurrentContestId()) ?? 0;


        protected BaseController(GrowDbContext dbContext)
        {
            DbContext = dbContext;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Default values for ViewBag
            ViewBag.TransparentNavbar = false;

            base.OnActionExecuting(context);
        }


        protected int GetCurrentContestId()
        {
            // TODO: choose contest according to settings
            return DbContext.Contests.First().Id;
        }
    }
}