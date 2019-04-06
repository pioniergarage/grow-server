﻿using System;
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
    public class EventsController : BaseAdminController
    {
        public EventsController(GrowDbContext context) : base(context)
        {
        }

        // GET: Admin/Events
        public async Task<IActionResult> Index()
        {
            return View(await Context.Events.ToListAsync().ConfigureAwait(false));
        }

        // GET: Admin/Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await Context.Events
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Admin/Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Start,End,FacebookLink,Location,Address,Visibility,Type,HasTimesSet,IsMandatory")] Event @event)
        {
            if (ModelState.IsValid)
            {
                Context.Add(@event);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Admin/Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await Context.Events.FindAsync(id).ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Admin/Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Start,End,FacebookLink,Location,Address,Visibility,Type,HasTimesSet,IsMandatory,Id")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(@event);
                    await Context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Admin/Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await Context.Events
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Admin/Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await Context.Events.FindAsync(id).ConfigureAwait(false);
            Context.Events.Remove(@event);
            await Context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return Context.Events.Any(e => e.Id == id);
        }
    }
}
