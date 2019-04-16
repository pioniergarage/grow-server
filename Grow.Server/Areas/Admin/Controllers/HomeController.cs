using Grow.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseAdminController
    {
        public HomeController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            var query = DbContext.Contests.Where(c => c.Year == CurrentContestYear);
            query = query
                .Include(c => c.Events)
                .Include(c => c.Mentors)
                    .ThenInclude(m => m.Person)
                .Include(c => c.Prizes)
                .Include(c => c.Teams);
            var contest = query.Single();

            ViewBag.CurrentContestYear = contest.Year;
            ViewBag.LatestContestYear = DbContext.Contests.OrderByDescending(c => c.Year).First().Year;

            return View(contest);
        }
    }
}
