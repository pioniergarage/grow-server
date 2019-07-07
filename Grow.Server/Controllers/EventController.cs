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

            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;
            return View(evnt);
        }

        public async Task<IActionResult> Respond(int id)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!CheckEventVisibility(evnt))
                return Unauthorized();

            // Default values
            var model = new EventResponse()
            {
                Event = evnt,
                EventId = evnt.Id,
                ParticipantCount = 1
            };

            // Logged in => Prefill name and email
            if (User.Identity.IsAuthenticated)
            {
                var user = await GetCurrentUserAsync().ConfigureAwait(false);
                model.Name = user.UserName;
                model.Email = user.Email;
            }

            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Respond(EventResponse response)
        {
            var evnt = DbContext.Events.Find(response.EventId);
            if (evnt == null)
                return NotFound();
            if (!CheckEventVisibility(evnt))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                response.Event = evnt;
                ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;
                return View(response);
            }

            // Logged in => auto-set name and email
            if (User.Identity.IsAuthenticated)
            {
                var user = await GetCurrentUserAsync().ConfigureAwait(false);
                response.Name = user.UserName;
                response.Email = user.Email;
            }

            // Set other values
            response.Created = DateTime.UtcNow;
            response.Id = 0;

            // Save to db
            DbContext.Add(response);
            DbContext.SaveChanges();
            return View("ResponseConfirmation");
        }

        private Task<Account> GetCurrentUserAsync()
        {
            return UserManager.GetUserAsync(User);
        }

        private bool CheckEventVisibility(Event evnt)
        {
            if (evnt == null)
                return false;

            return evnt.CanUserRegisterNow(User.Identity.IsAuthenticated);
        }
    }
}