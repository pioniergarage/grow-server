using Grow.Data;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Grow.Server.Controllers
{
    public abstract class BaseFrontendController : BaseController
    {
        protected BaseFrontendController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
            GlobalFilter = (e => e.IsActive);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Default values for all controller actions
            ViewBag.TransparentNavbar = false;
        }
    }
}