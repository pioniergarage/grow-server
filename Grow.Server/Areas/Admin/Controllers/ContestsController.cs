using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Grow.Server.Model.Helpers;
using System;

namespace Grow.Server.Areas.Admin.Controllers
{
    public class ContestsController : BaseAdminController
    {
        public ContestsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await DbContext.Contests.ToListAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await DbContext.Contests
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contest contest)
        {
            if (ModelState.IsValid)
            {
                DbContext.Add(contest);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await DbContext.Contests.FindAsync(id).ConfigureAwait(false);
            if (contest == null)
            {
                return NotFound();
            }
            return View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contest contest)
        {
            if (id != contest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldContest = DbContext.Contests.Find(id);
                    DbContext.Entry(oldContest).State = EntityState.Detached;
                    DbContext.Attach(contest);
                    DbContext.Entry(contest).State = EntityState.Modified;
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestExists(contest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contest = await DbContext.Contests
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contest = await DbContext.Contests.FindAsync(id).ConfigureAwait(false);
            DbContext.Contests.Remove(contest);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Contests.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool ContestExists(int id)
        {
            return DbContext.Contests.Any(e => e.Id == id);
        }
    }
}
