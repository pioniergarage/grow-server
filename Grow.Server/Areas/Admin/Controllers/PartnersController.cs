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
    public class PartnersController : BaseAdminController
    {
        public PartnersController(GrowDbContext context) : base(context)
        {
        }

        // GET: Admin/Partners
        public async Task<IActionResult> Index()
        {
            return View(await Context.Partners.ToListAsync().ConfigureAwait(false));
        }

        // GET: Admin/Partners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await Context.Partners
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // GET: Admin/Partners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Id")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                Context.Add(partner);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        // GET: Admin/Partners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await Context.Partners.FindAsync(id).ConfigureAwait(false);
            if (partner == null)
            {
                return NotFound();
            }
            return View(partner);
        }

        // POST: Admin/Partners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id")] Partner partner)
        {
            if (id != partner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(partner);
                    await Context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerExists(partner.Id))
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
            return View(partner);
        }

        // GET: Admin/Partners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await Context.Partners
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // POST: Admin/Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await Context.Partners.FindAsync(id).ConfigureAwait(false);
            Context.Partners.Remove(partner);
            await Context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerExists(int id)
        {
            return Context.Partners.Any(e => e.Id == id);
        }
    }
}
