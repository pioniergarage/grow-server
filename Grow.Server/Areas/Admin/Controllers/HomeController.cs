using Grow.Data;
using Grow.Server.Areas.Admin.Model.ViewModels;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseAdminController
    {
        public HomeController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            var model = new DashboardViewModel(DbContext, SelectedContestYear);

            return View(model);
        }

        [HttpPost]
        public IActionResult SetYear(string year)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(6)
            };
            
            if (ContestYears.Any(c => c.Value.Equals(year)))
            {
                Response.Cookies.Append(Constants.COOKIE_SELECTED_YEAR_KEY, year, options);
                return Ok();
            }

            return NotFound();
        }
    }
}
