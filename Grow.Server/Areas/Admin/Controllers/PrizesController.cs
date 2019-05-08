using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrizesController : BaseAdminController
    {
        public PrizesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await PrizesInSelectedYear.ToListAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await DbContext.Prizes
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (prize == null)
            {
                return NotFound();
            }

            return View(prize);
        }

        public IActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Reward,RewardValue,Description,IsPublic,Type,Id")] Prize prize)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Prizes).Single().Prizes.Add(prize);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
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
            return View(prize);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Reward,RewardValue,Description,IsPublic,Type,Id")] Prize prize)
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
            return View(prize);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await DbContext.Prizes
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

        private bool PrizeExists(int id)
        {
            return DbContext.Prizes.Any(e => e.Id == id);
        }
    }
}
