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
    public class MentorsController : BaseAdminController
    {
        private IQueryable<Mentor> SelectedMentorsWithAllIncluded => MentorsInSelectedYear
            .Include(t => t.Contest)
            .Include(t => t.Image);

        public MentorsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await SelectedMentorsWithAllIncluded.ToListAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentor = await SelectedMentorsWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (mentor == null)
            {
                return NotFound();
            }

            return View(mentor);
        }

        public IActionResult Create()
        {
            AddEntityListsToViewBag();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mentor mentor)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Mentors).Single().Mentors.Add(mentor);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            AddEntityListsToViewBag();
            return View(mentor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentor = await DbContext.Mentors.FindAsync(id).ConfigureAwait(false);
            if (mentor == null)
            {
                return NotFound();
            }

            AddEntityListsToViewBag();
            return View(mentor);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mentor mentor)
        {
            if (id != mentor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(mentor);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MentorExists(mentor.Id))
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
            return View(mentor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentor = await SelectedMentorsWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (mentor == null)
            {
                return NotFound();
            }

            return View(mentor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mentor = await DbContext.Mentors.FindAsync(id).ConfigureAwait(false);
            DbContext.Mentors.Remove(mentor);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Mentors.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void AddEntityListsToViewBag()
        {
            ViewBag.Images = ViewHelpers.SelectListFromFiles<Person>(DbContext, p => p.Image);
        }

        private bool MentorExists(int id)
        {
            return DbContext.Mentors.Any(e => e.Id == id);
        }
    }
}
