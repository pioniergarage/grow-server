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
        private Lazy<StorageConnector> Storage { get; set; }

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
            Storage = new Lazy<StorageConnector>(() => new StorageConnector(AppSettings, logger));
        }

        public IActionResult Index()
        {
            var list = VisibleEvents.ToList();
            return View(list);
        }
        
        public IActionResult Respond(int id)
        {
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!evnt.CanTeamRespondNow(MyTeam))
                return View("Closed", evnt);

            // Has already responded?
            var oldResponse = (TeamResponse) DbContext.EventResponses.FirstOrDefault(e =>
                e is TeamResponse
                && ((TeamResponse)e).TeamId == MyTeamId
                && e.EventId == evnt.Id);

            // Default values
            var model = oldResponse != null
                ? new TeamResponseViewModel(oldResponse)
                : new TeamResponseViewModel()
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
        public IActionResult Respond(int id, TeamResponseViewModel vm)
        {
            // Check input
            var evnt = DbContext.Events.Find(id);
            if (evnt == null)
                return NotFound();
            if (!evnt.CanTeamRespondNow(MyTeam))
                return Unauthorized();
            if (evnt.TeamRegistrationOptions.AcceptFileUploads && vm.UploadedFile == null && vm.ExternalFileUrl == null)
                ModelState.AddModelError(nameof(vm.ExternalFileUrl), "No submission has been selected");

            // if not valid, return
            if (!ModelState.IsValid)
            {
                vm.EventId = evnt.Id;
                vm.Event = evnt;
                vm.TeamName = MyTeam.Name;
                return View(vm);
            }

            // Save file (if uploaded)
            string fileUrl = vm.ExternalFileUrl;
            if (evnt.TeamRegistrationOptions.AcceptFileUploads && vm.UploadedFile != null)
            {
                if (!evnt.TeamRegistrationOptions.AllowedFileExtensions.Any(e => vm.UploadedFile.FileName.EndsWith(e)))
                {
                    ModelState.AddModelError(nameof(vm.ExternalFileUrl), "Only the following file types are allowed: " + evnt.TeamRegistrationOptions.AllowedFileExtensionsString);
                    vm.EventId = evnt.Id;
                    vm.Event = evnt;
                    vm.TeamName = MyTeam.Name;
                    return View(vm);
                }

                using (var stream = vm.UploadedFile.OpenReadStream())
                {
                    var file = Storage.Value.Create(
                        nameof(FileCategory.Submissions),
                        string.Format(
                            "submit-{0}-{1}-{2}-{3}",
                            SelectedContestYear,
                            evnt.Name.Replace(" ", "").ToLower(), 
                            MyTeam.Name.Replace(" ","").ToLower(), 
                            vm.UploadedFile.FileName
                        ),
                        stream
                    );
                    fileUrl = file.Url;
                    DbContext.Files.Add(file);
                }
            }

            // Has already responded?
            var response = (TeamResponse)DbContext.EventResponses.FirstOrDefault(e =>
               e is TeamResponse
               && ((TeamResponse)e).TeamId == MyTeamId
               && e.EventId == evnt.Id);

            // Save response
            if (response == null)
            {
                response = new TeamResponse
                {
                    EventId = evnt.Id,
                    TeamId = MyTeamId,
                };
                DbContext.EventResponses.Add(response);
            }
            response.ParticipantCount = vm.ParticipantCount;
            response.FileUrl = fileUrl;

            // Save changes
            DbContext.SaveChanges();

            return View("ResponseConfirmation");
        }

        private void AddEntityListsToViewBag()
        {
            
        }
    }
}
