using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Data;
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
            _mapper = new TeamVmMapper(DbContext);
        }

        public IActionResult Index()
        {
            var team = MyTeamQuery
                .Include(t => t.Contest)
                .Include(t => t.TeamPhoto)
                .Include(t => t.LogoImage)
                .Single();

            var vm = _mapper.TeamToViewModel(team);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            
            var oldTeam = DbContext.Teams.Find(vm.Id);
            if (oldTeam == null)
                return NotFound();
            
            _mapper.UpdateTeam(oldTeam, vm);
            ViewBag.Success = true;
            return View(vm);
        }
    }
}