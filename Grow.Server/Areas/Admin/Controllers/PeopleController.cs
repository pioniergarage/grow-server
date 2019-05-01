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
    public class PeopleController : BaseAdminController
    {
        private const string DEFAULT_RETURN_URL = "/Admin/Home/Index";

        public PeopleController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }
        
        public async Task<IActionResult> Details(int? id, string ReturnUrl = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await DbContext.Persons
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = ReturnUrl ?? DEFAULT_RETURN_URL;
            return View(person);
        }
        
        public async Task<IActionResult> Edit(int? id, string ReturnUrl = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await DbContext.Persons.FindAsync(id).ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = ReturnUrl ?? DEFAULT_RETURN_URL;
            return View(person);
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,JobTitle,Description,Expertise,Email,WebsiteUrl,Id")] Person person, string ReturnUrl = null)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(person);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
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
                return Redirect(ReturnUrl ?? DEFAULT_RETURN_URL);
            }

            ViewBag.ReturnUrl = ReturnUrl ?? DEFAULT_RETURN_URL;
            return View(person);
        }
        
        public async Task<IActionResult> Delete(int? id, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await DbContext.Persons
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = ReturnUrl ?? DEFAULT_RETURN_URL;
            return View(person);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string ReturnUrl)
        {
            var person = await DbContext.Persons.FindAsync(id).ConfigureAwait(false);
            DbContext.Persons.Remove(person);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(ReturnUrl ?? DEFAULT_RETURN_URL);
        }

        private bool PersonExists(int id)
        {
            return DbContext.Persons.Any(e => e.Id == id);
        }
    }
}
