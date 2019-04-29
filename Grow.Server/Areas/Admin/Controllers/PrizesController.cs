using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Server.Model.Entities;
using Microsoft.Extensions.Options;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrizesController : BaseAdminController
    {
        public PrizesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        // GET: Admin/Prizes
        public async Task<IActionResult> Index()
        {
            return View(await DbContext.Prizes.ToListAsync().ConfigureAwait(false));
        }

        // GET: Admin/Prizes/Details/5
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

        // GET: Admin/Prizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Prizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Reward,RewardValue,Description,IsPublic,Type,Id")] Prize prize)
        {
            if (ModelState.IsValid)
            {
                DbContext.Add(prize);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(prize);
        }

        // GET: Admin/Prizes/Edit/5
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

        // POST: Admin/Prizes/Edit/5
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

        // GET: Admin/Prizes/Delete/5
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

        // POST: Admin/Prizes/Delete/5
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
