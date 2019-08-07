using Grow.Data;
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
            var query = DbContext.Contests.Where(c => c.Year == SelectedContestYear);
            query = query
                .Include(c => c.Events)
                .Include(c => c.Mentors)
                .Include(c => c.Prizes)
                .Include(c => c.Teams);
            var contest = query.Single();

            ViewBag.SelectedContestYear = contest.Year;
            ViewBag.LatestContestYear = DbContext.Contests.AsNoTracking().OrderByDescending(c => c.Year).First().Year;

            return View(contest);
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
