using System.Linq;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Grow.Server.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Controllers
{
    public class HomeController : BaseFrontendController
    {
        public HomeController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                Partners = PartnersInSelectedYear
                .Include(t => t.Image)
                .ToList(),
                MainEvents = EventsInSelectedYear
                .Include(e => e.Image)
                .Where(e => e.Type == Event.EventType.MainEvent)
                .ToList(),
                Contest = SelectedContest.Single()
            };
            return View(model);
        }
        
        [Route("/Error")]
        public IActionResult Error(ErrorViewModel model)
        {
            return View(model);
        }

        [Route("/ErrorCode")]
        public IActionResult ErrorCode(string code)
        {
            return View("Error", ErrorViewModel.FromStatusCode(code));
        }

        public IActionResult Newsletter()
        {
            return View();
        }

        public IActionResult Teams()
        {
            var teams = TeamsInSelectedYear
                .Include(t => t.LogoImage)
                .Include(t => t.TeamPhoto)
                .OrderByDescending(t => !t.HasDroppedOut)
                    .ThenBy(t=> t.Name)
                .ToList();

            var prizes = PrizesInSelectedYear
                .Include(p => p.Winner)
                .Include(p => p.GivenBy)
                    .ThenInclude(p => p.Image)
                .OrderByDescending(p => p.GivenBy == null)
                    .ThenByDescending(p => p.RewardValue)
                .ToList();

            return View(new TeamsViewModel() { Teams = teams, Prizes = prizes });
        }

        public IActionResult Judges()
        {
            var model = JudgesInSelectedYear
                .Include(p => p.Image)
                .OrderBy(p => p.Name)
                .ToList();
            return View(model);
        }

        public IActionResult About()
        {
            var model = OrganizersInSelectedYear
                .Include(p => p.Image)
                .OrderBy(p => p.Name)
                .ToList();
            return View(model);
        }

        public IActionResult Mentors()
        {
            var model = MentorsInSelectedYear
                .Include(p => p.Image)
                .OrderBy(p => p.Name)
                .ToList();
            return View(model);
        }

        public IActionResult Program()
        {
            var model = EventsInSelectedYear
                .Include(e => e.HeldBy)
                .OrderBy(e => e.Start)
                .ToList();

            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;
            return View(model);
        }
    }
}