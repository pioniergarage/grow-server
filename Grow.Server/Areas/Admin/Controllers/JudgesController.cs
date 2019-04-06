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
    public class JudgesController : BaseAdminController
    {
        public JudgesController(GrowDbContext context) : base(context)
        {
        }

        // GET: Admin/People
        public async Task<IActionResult> Index()
        {
            return View(await Context.Persons.ToListAsync().ConfigureAwait(false));
        }

        // GET: Admin/People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await Context.Persons
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Admin/People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,JobTitle,Description,Expertise,Email,WebsiteUrl,Id")] Person person)
        {
            if (ModelState.IsValid)
            {
                Context.Add(person);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Admin/People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await Context.Persons.FindAsync(id).ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Admin/People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,JobTitle,Description,Expertise,Email,WebsiteUrl,Id")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(person);
                    await Context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: Admin/People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await Context.Persons
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Admin/People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await Context.Persons.FindAsync(id).ConfigureAwait(false);
            Context.Persons.Remove(person);
            await Context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return Context.Persons.Any(e => e.Id == id);
        }
    }
}
