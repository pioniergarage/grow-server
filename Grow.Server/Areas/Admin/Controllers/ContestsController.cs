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
    public class ContestsController : BaseAdminController
    {
        public ContestsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        // GET: Admin/Contests
        public async Task<IActionResult> Index()
        {
            return View(await DbContext.Contests.ToListAsync().ConfigureAwait(false));
        }

        // GET: Admin/Contests/Details/5
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

        // GET: Admin/Contests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Year,Language")] Contest contest)
        {
            if (ModelState.IsValid)
            {
                DbContext.Add(contest);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }

        // GET: Admin/Contests/Edit/5
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

        // POST: Admin/Contests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Year,Language,Id")] Contest contest)
        {
            if (id != contest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(contest);
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
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }

        // GET: Admin/Contests/Delete/5
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

        // POST: Admin/Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contest = await DbContext.Contests.FindAsync(id).ConfigureAwait(false);
            DbContext.Contests.Remove(contest);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool ContestExists(int id)
        {
            return DbContext.Contests.Any(e => e.Id == id);
        }
    }
}
