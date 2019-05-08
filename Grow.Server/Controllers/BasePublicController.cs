using Grow.Data;
using Grow.Server.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Grow.Server.Controllers
{
    public abstract class BasePublicController : BaseController
    {
        protected BasePublicController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Default values for all controller actions
            ViewBag.TransparentNavbar = false;
        }
    }
}