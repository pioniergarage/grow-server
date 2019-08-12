using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.Extensions.Options;
using Grow.Server.Areas.MyTeam.Model;
using Grow.Server.Areas.MyTeam.Model.ViewModels;
using Grow.Server.Model.Extensions;

namespace Grow.Server.Areas.MyTeam.Controllers
{
    public class EventsController : BaseTeamController
    {
        private Team _myTeam;
        private Team MyTeam
        {
            get
            {
                return _myTeam
                    ?? (_myTeam = MyTeamQuery.Include(t => t.Contest).Single());
            }
        }

        private IQueryable<TeamEventViewModel> VisibleEvents => DbContext.Events
            .Where(e => e.Contest.Year == MyTeam.Contest.Year && e.IsActive) // teams can see active events in the same year
            .Include(e => e.Contest)
            .Include(e => e.HeldBy)
            .Include(e => e.Image)
            .Include(e => e.Slides)
            .Include(e => e.Responses)
            .Select(e => new TeamEventViewModel(e, MyTeam));
        
        public EventsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) : base(dbContext, appSettings, logger)
        {
        }

        public IActionResult Index()
        {
            var list = VisibleEvents.ToList();
            return View(list);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evnt = VisibleEvents.FirstOrDefault(e => e.Id == id);
            if (evnt == null)
            {
                return NotFound();
            }

            return View(evnt);
        }

        public IActionResult Respond(int id)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!evnt.CanTeamRespondNow(MyTeam))
                return View("Closed", evnt);

            // Default values
            var model = new TeamResponseViewModel()
            {
                Event = evnt,
                EventId = evnt.Id,
                TeamName = MyTeam.Name,
                ParticipantCount = 1
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Respond(int id, TeamResponseViewModel response)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!evnt.CanTeamRespondNow(MyTeam))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                response.EventId = evnt.Id;
                response.Event = evnt;
                response.TeamName = MyTeam.Name;
                return View(response);
            }

            /*
            DbContext.Add(response);
            DbContext.SaveChanges();
            */
            return View("ResponseConfirmation");
        }

        private void AddEntityListsToViewBag()
        {
            
        }
    }
}
