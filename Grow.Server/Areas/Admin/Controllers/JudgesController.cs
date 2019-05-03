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
    public class JudgesController : BaseAdminController
    {
        public JudgesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await JudgesInSelectedYear.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await DbContext.Judges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (judge == null)
            {
                return NotFound();
            }

            return View(judge);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WebsiteUrl,Name,JobTitle,Description,Email,Id,CreatedAt,UpdatedAt")] Judge judge)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Judges).Single().Judges.Add(judge);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(judge);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await DbContext.Judges.FindAsync(id);
            if (judge == null)
            {
                return NotFound();
            }
            return View(judge);
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WebsiteUrl,Name,JobTitle,Description,Email,Id,CreatedAt,UpdatedAt")] Judge judge)
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
                    await DbContext.SaveChangesAsync();
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
            return View(judge);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await DbContext.Judges
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var judge = await DbContext.Judges.FindAsync(id);
            DbContext.Judges.Remove(judge);
            await DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JudgeExists(int id)
        {
            return DbContext.Judges.Any(e => e.Id == id);
        }
    }
}
