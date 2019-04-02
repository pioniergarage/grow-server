using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Server.Model.Entities;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrizesController : Controller
    {
        private readonly GrowDbContext _context;

        public PrizesController(GrowDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Prizes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Prizes.ToListAsync());
        }

        // GET: Admin/Prizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await _context.Prizes
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(prize);
                await _context.SaveChangesAsync();
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

            var prize = await _context.Prizes.FindAsync(id);
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
                    _context.Update(prize);
                    await _context.SaveChangesAsync();
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

            var prize = await _context.Prizes
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var prize = await _context.Prizes.FindAsync(id);
            _context.Prizes.Remove(prize);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrizeExists(int id)
        {
            return _context.Prizes.Any(e => e.Id == id);
        }
    }
}
