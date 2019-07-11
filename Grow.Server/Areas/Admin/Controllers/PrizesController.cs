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
    public class PrizesController : BaseAdminController
    {
        private IQueryable<Prize> SelectedPrizesWithAllIncluded => PrizesInSelectedYear
            .Include(t => t.Contest)
            .Include(t => t.GivenBy)
            .Include(t => t.Winner);

        public PrizesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await SelectedPrizesWithAllIncluded.ToListAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await SelectedPrizesWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (prize == null)
            {
                return NotFound();
            }

            return View(prize);
        }

        public IActionResult Create()
        {
            AddEntityListsToViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prize prize)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Prizes).Single().Prizes.Add(prize);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            AddEntityListsToViewBag();
            return View(prize);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await DbContext.Prizes.FindAsync(id).ConfigureAwait(false);
            if (prize == null)
            {
                return NotFound();
            }

            AddEntityListsToViewBag();
            return View(prize);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prize prize)
        {
            if (id != prize.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(prize);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrizeExists(prize.Id))
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
            return View(prize);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await SelectedPrizesWithAllIncluded
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (prize == null)
            {
                return NotFound();
            }

            return View(prize);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prize = await DbContext.Prizes.FindAsync(id).ConfigureAwait(false);
            DbContext.Prizes.Remove(prize);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Prizes.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void AddEntityListsToViewBag()
        {
            ViewBag.Types = ViewHelpers.SelectListFromEnum<Prize.PrizeType>();
            ViewBag.Teams = ViewHelpers.SelectListFromEntities<Data.Entities.Team>(DbContext, SelectedContestId);
            ViewBag.Partners = ViewHelpers.SelectListFromEntities<Partner>(DbContext, SelectedContestId);
        }

        private bool PrizeExists(int id)
        {
            return DbContext.Prizes.Any(e => e.Id == id);
        }
    }
}
