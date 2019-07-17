using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.Team.Model;
using Grow.Server.Areas.Team.Model.ViewModels;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.Team.Controllers
{
    public class HomeController : BaseTeamController
    {
        private TeamVmMapper _mapper { get; }

        public HomeController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) 
            : base(dbContext, appSettings, logger)
        {
            var storage = new StorageConnector(appSettings.Value, logger);
            _mapper = new TeamVmMapper(DbContext, storage);
        }

        public IActionResult Index()
        {
            var team = MyTeamQuery
                .Include(t => t.Contest)
                .Single();
            var vm = _mapper.TeamToViewModel(team);

            FillViewBag();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                FillViewBag();
                return View(vm);
            }

            var oldTeam = MyTeamQuery.Single();
            if (oldTeam.Id != vm.Id)
                return Unauthorized();
            
            _mapper.UpdateTeam(oldTeam, vm);
            ViewBag.Success = true;

            FillViewBag();
            return View(vm);
        }

        private void FillViewBag()
        {
            ViewBag.TeamPhotoUrl = MyTeamQuery.Select(t => t.TeamPhoto).Select(f => f.Url).SingleOrDefault();
            ViewBag.LogoImageUrl = MyTeamQuery.Select(t => t.LogoImage).Select(f => f.Url).SingleOrDefault();
        }
    }
}