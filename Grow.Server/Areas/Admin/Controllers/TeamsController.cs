﻿using System;
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
    public class TeamsController : BaseAdminController
    {
        public TeamsController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await TeamsInSelectedYear.ToListAsync().ConfigureAwait(false));
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await DbContext.Teams
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TagLine,Description,ActiveSince,WebsiteUrl,Email,FacebookUrl,InstagramUrl,IsActive,MembersAsString,Id")] Team team)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Teams).Single().Teams.Add(team);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await DbContext.Teams.FindAsync(id).ConfigureAwait(false);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,TagLine,Description,ActiveSince,WebsiteUrl,Email,FacebookUrl,InstagramUrl,IsActive,MembersAsString,Id")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(team);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
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
            return View(team);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await DbContext.Teams
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await DbContext.Teams.FindAsync(id).ConfigureAwait(false);
            DbContext.Teams.Remove(team);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return DbContext.Teams.Any(e => e.Id == id);
        }
    }
}
