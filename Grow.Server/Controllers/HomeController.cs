using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
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
            return View();
        }

        public IActionResult Teams()
        {
            var model = DbContext.Teams
                .Where(t => t.Contest.Id == CurrentContestId)
                .Include(t => t.LogoImage)
                .Include(t => t.TeamPhoto)
                .OrderByDescending(t => t.IsActive)
                    .ThenBy(t=> t.TeamName)
                .ToList();
            return View(model);
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