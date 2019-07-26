using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Grow.Server.Model.Extensions;

namespace Grow.Server.Controllers
{
    public class EventController : BaseFrontendController
    {
        public UserManager<Account> UserManager { get; }

        public EventController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger, UserManager<Account> userManager) 
            : base(dbContext, appSettings, logger)
        {
            UserManager = userManager;
        }
        
        public IActionResult View(int id)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            
            return View(evnt);
        }

        public IActionResult Respond(int id)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!evnt.CanVisitorRespondNow())
                return View("Closed", evnt);

            // Default values
            var model = new VisitorResponse()
            {
                Event = evnt,
                EventId = evnt.Id,
                ParticipantCount = 1
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Respond(int id, VisitorResponse response)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!evnt.CanVisitorRespondNow())
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                response.EventId = evnt.Id;
                response.Event = evnt;
                return View(response);
            }

            // Set other values
            response.Created = DateTime.UtcNow;
            response.Id = 0;

            // Save to db
            DbContext.Add(response);
            DbContext.SaveChanges();
            return View("ResponseConfirmation");
        }
    }
}