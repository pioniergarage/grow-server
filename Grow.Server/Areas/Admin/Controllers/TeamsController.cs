using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Grow.Server.Model.Helpers;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : BaseAdminController
    {
        private IQueryable<Team> SelectedTeamsWithAllIncluded => TeamsInSelectedYear
            .Include(t => t.Contest)
            .Include(t => t.LogoImage)
            .Include(t => t.TeamPhoto);

        public TeamsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            var teams = await SelectedTeamsWithAllIncluded.ToListAsync().ConfigureAwait(false);
            
            return View(teams);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await SelectedTeamsWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (team == null)
            {
                return NotFound();
            }
            
            return View(team);
        }

        public IActionResult Create()
        {
            AddEntityListsToViewBag();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Teams).Single().Teams.Add(team);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            AddEntityListsToViewBag();
            return View(team);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await DbContext.Teams.FindAsync(id).ConfigureAwait(false);
            if (team == null)
            {
                return NotFound();
            }
            AddEntityListsToViewBag();
            return View(team);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(team);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            AddEntityListsToViewBag();
            return View(team);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await SelectedTeamsWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (team == null)
            {
                return NotFound();
            }
            
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await DbContext.Teams.FindAsync(id).ConfigureAwait(false);
            DbContext.Teams.Remove(team);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Teams.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void AddEntityListsToViewBag()
        {
            ViewBag.Images = ViewHelpers.SelectListFromEntities<Image>(DbContext);
        }

        private bool TeamExists(int id)
        {
            return DbContext.Teams.Any(e => e.Id == id);
        }
    }
}
