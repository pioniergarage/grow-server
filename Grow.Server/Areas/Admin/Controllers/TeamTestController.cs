using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Data;
using Grow.Data.Entities;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamTestController : Controller
    {
        private readonly GrowDbContext _context;

        public TeamTestController(GrowDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TeamTest
        public async Task<IActionResult> Index()
        {
            var growDbContext = _context.Teams.Include(t => t.Contest).Include(t => t.LogoImage);
            return View(await growDbContext.ToListAsync());
        }

        // GET: Admin/TeamTest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Contest)
                .Include(t => t.LogoImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Admin/TeamTest/Create
        public IActionResult Create()
        {
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Id");
            ViewData["LogoImageId"] = new SelectList(_context.Images, "Id", "Id");
            return View();
        }

        // POST: Admin/TeamTest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagLine,Description,LogoImageId,TeamPhotoId,ActiveSince,WebsiteUrl,Email,FacebookUrl,InstagramUrl,HasDroppedOut,MembersAsString,ContestId,Id,Name,IsActive,CreatedAt,UpdatedAt")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Id", team.ContestId);
            ViewData["LogoImageId"] = new SelectList(_context.Images, "Id", "Id", team.LogoImageId);
            return View(team);
        }

        // GET: Admin/TeamTest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Id", team.ContestId);
            ViewData["LogoImageId"] = new SelectList(_context.Images, "Id", "Id", team.LogoImageId);
            return View(team);
        }

        // POST: Admin/TeamTest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagLine,Description,LogoImageId,TeamPhotoId,ActiveSince,WebsiteUrl,Email,FacebookUrl,InstagramUrl,HasDroppedOut,MembersAsString,ContestId,Id,Name,IsActive,CreatedAt,UpdatedAt")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
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
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Id", team.ContestId);
            ViewData["LogoImageId"] = new SelectList(_context.Images, "Id", "Id", team.LogoImageId);
            return View(team);
        }

        // GET: Admin/TeamTest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Contest)
                .Include(t => t.LogoImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Admin/TeamTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
