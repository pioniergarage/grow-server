using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
using Grow.Server.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grow.Server.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(GrowDbContext dbContext) : base(dbContext)
        {
        }


        public IActionResult Index()
        {
            var model = DbContext.Partners
                .Where(p => DbContext.Contests.Any(c => c.Id == CurrentContestId && c.Partners.Any(l => l.Partner.Equals(p))))
                .Include(t => t.Image)
                .ToList();
            return View(model);
        }

        public IActionResult Teams()
        {
            var teams = DbContext.Teams
                .Where(t => t.Contest.Id == CurrentContestId)
                .Include(t => t.LogoImage)
                .Include(t => t.TeamPhoto)
                .OrderByDescending(t => t.IsActive)
                    .ThenBy(t=> t.Name)
                .ToList();

            var prizes = DbContext.Prizes
                .Where(p => p.Contest.Id == CurrentContestId)
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
            var model = DbContext.Persons
                .Where(p => DbContext.Contests.Any(c => c.Id == CurrentContestId && c.Judges.Any(l => l.Person.Equals(p))))
                .Include(p => p.Image)
                .OrderBy(p => p.Name)
                .ToList();
            return View(model);
        }

        public IActionResult Organizers()
        {
            var model = DbContext.Persons
                .Where(p => DbContext.Contests.Any(c => c.Id == CurrentContestId && c.Organizers.Any(l => l.Person.Equals(p))))
                .Include(p => p.Image)
                .OrderBy(p => p.Name)
                .ToList();
            return View(model);
        }

        public IActionResult Mentors()
        {
            var model = DbContext.Persons
                .Where(p => DbContext.Contests.Any(c => c.Id == CurrentContestId && c.Mentors.Any(l => l.Person.Equals(p))))
                .Include(p => p.Image)
                .OrderBy(p => p.Name)
                .ToList();
            return View(model);
        }

        public IActionResult Program()
        {
            var model = DbContext.Events
                .Where(e => e.Contest.Id == CurrentContestId)
                .Include(e => e.HeldBy)
                .OrderBy(e => e.Start)
                .ToList();
            return View(model);
        }

    }
}