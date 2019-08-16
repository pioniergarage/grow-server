using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.MyTeam.Model;
using Grow.Server.Areas.MyTeam.Model.ViewModels;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.MyTeam.Controllers
{
    public class HomeController : BaseTeamController
    {
        private TeamVmMapper _mapper { get; }

        private Team MyTeam
        {
            get
            {
                return _team 
                    ?? (_team = MyTeamQuery
                        .Include(t => t.Contest)
                        .Include(t => t.LogoImage)
                        .Include(t => t.TeamPhoto)
                        .Single());
            }
        }
        private Team _team;

        public HomeController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) 
            : base(dbContext, appSettings, logger)
        {
            var storage = new Lazy<StorageConnector>(() => new StorageConnector(appSettings.Value, logger));
            _mapper = new TeamVmMapper(DbContext, storage);
        }

        public IActionResult Index()
        {
            var teamVm = _mapper.TeamToViewModel(MyTeam);

            var eventVms = DbContext.Events
            .Where(e => e.Contest.Year == MyTeam.Contest.Year && e.IsActive && e.Start > DateTime.Now)
            .Include(e => e.Contest)
            .Include(e => e.HeldBy)
            .Include(e => e.Image)
            .Include(e => e.Responses)
            .Select(e => new TeamEventViewModel(e, MyTeam));

            var vm = new TeamIndexViewModel()
            {
                MyTeam = teamVm,
                UpcomingEvents = eventVms
            };

            FillViewBag();
            return View(vm);
        }

        public IActionResult Profile()
        {
            var vm = _mapper.TeamToViewModel(MyTeam);

            FillViewBag();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(TeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                FillViewBag();
                return View(vm);
            }
            
            if (MyTeam.Id != vm.Id)
                return Unauthorized();
            
            _mapper.UpdateTeam(MyTeam, vm);
            ViewBag.Success = true;

            FillViewBag();
            return View(vm);
        }

        private void FillViewBag()
        {
        }
    }
}