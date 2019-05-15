using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PartnersController : BaseAdminController
    {
        public PartnersController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await PartnersInSelectedYear.ToListAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await DbContext.Partners
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        public IActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsActive,Name,Description,Id")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                SelectedContest.Include(c => c.Partners).Single().Partners.Add(partner);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await DbContext.Partners.FindAsync(id).ConfigureAwait(false);
            if (partner == null)
            {
                return NotFound();
            }
            return View(partner);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IsActive,Name,Description,Id")] Partner partner)
        {
            if (id != partner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(partner);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await DbContext.Partners
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await DbContext.Partners.FindAsync(id).ConfigureAwait(false);
            DbContext.Partners.Remove(partner);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Partners.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool PartnerExists(int id)
        {
            return DbContext.Partners.Any(e => e.Id == id);
        }
    }
}
