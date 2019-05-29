using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Grow.Server.Model.Helpers;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JudgesController : BaseAdminController
    {
        private IQueryable<Judge> SelectedJudgesWithAllIncluded => JudgesInSelectedYear
            .Include(t => t.Contest)
            .Include(t => t.Image);

        public JudgesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await SelectedJudgesWithAllIncluded.ToListAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await SelectedJudgesWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (judge == null)
            {
                return NotFound();
            }

            return View(judge);
        }

        public IActionResult Create()
        {
            AddEntityListsToViewBag();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Judge judge)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Judges).Single().Judges.Add(judge);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            AddEntityListsToViewBag();
            return View(judge);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await DbContext.Judges.FindAsync(id).ConfigureAwait(false);
            if (judge == null)
            {
                return NotFound();
            }

            AddEntityListsToViewBag();
            return View(judge);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Judge judge)
        {
            if (id != judge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(judge);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JudgeExists(judge.Id))
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
            return View(judge);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await SelectedJudgesWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (judge == null)
            {
                return NotFound();
            }

            return View(judge);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var judge = await DbContext.Judges.FindAsync(id).ConfigureAwait(false);
            DbContext.Judges.Remove(judge);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Judges.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void AddEntityListsToViewBag()
        {
            ViewBag.Images = ViewHelpers.SelectListFromEntities<Image>(DbContext);
        }

        private bool JudgeExists(int id)
        {
            return DbContext.Judges.Any(e => e.Id == id);
        }
    }
}
