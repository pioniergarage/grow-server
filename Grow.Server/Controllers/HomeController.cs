using System.Linq;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model;
using Grow.Server.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Controllers
{
    public class HomeController : BasePublicController
    {
        public HomeController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
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
                .Where(e => e.Type == Event.EventType.MainEvent)
                .ToList(),
                Contest = SelectedContest.Single()
            };
            return View(model);
        }

        public IActionResult Error(string ErrorMessage = null, string ErrorDetails = null)
        {
            var model = new ErrorViewModel()
            {
                ErrorMessage = ErrorMessage ?? "Unknown Server Error",
                ErrorDetails = ErrorDetails ?? string.Empty
            };
            return View(model);
        }

        public IActionResult Error(ErrorViewModel model)
        {
            return View(model);
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

        public IActionResult Organizers()
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
            return View(model);
        }
    }
}