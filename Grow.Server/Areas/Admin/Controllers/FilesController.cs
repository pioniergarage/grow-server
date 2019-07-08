using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.Extensions.Options;
using Grow.Server.Areas.Admin.Model.ViewModels;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FilesController : BaseAdminController
    {
        private StorageConnector Storage => _storage.Value;
        private readonly Lazy<StorageConnector> _storage;

        public FilesController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) 
            : base(dbContext, appSettings, logger)
        {
            _storage = new Lazy<StorageConnector>(() => new StorageConnector(AppSettings, Logger));
        }

        public IActionResult Index(PaginationOptions options)
        {
            AddEntityListsToViewBag();
            var entities = new PaginatedList<File>(DbContext.Files, options);

            ViewBag.PaginationOptions = options;
            ViewBag.CurrentPage = options.PageIndex;
            ViewBag.PageCount = entities.PageCount;
            return View(entities);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await DbContext.Files
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }
        
        public IActionResult Create()
        {
            AddEntityListsToViewBag();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FileCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var file = vm.ToFile(Storage);

                DbContext.Add(file);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            AddEntityListsToViewBag();
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await DbContext.Files.FindAsync(id).ConfigureAwait(false);
            if (file == null)
            {
                return NotFound();
            }
            AddEntityListsToViewBag();
            return View(file);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, File file)
        {
            if (id != file.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // ensure read-only fields are not changed
                var oldFile = DbContext.Files.Find(id);
                if (oldFile == null)
                    return NotFound();
                file.Extension = oldFile.Extension;
                file.Url = oldFile.Url;

                try
                {
                    DbContext.Update(file);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.Id))
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
            AddEntityListsToViewBag();
            return View(file);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await DbContext.Files
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await DbContext.Files.FindAsync(id).ConfigureAwait(false);
            DbContext.Files.Remove(file);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            Storage.Delete(file);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Files.FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void AddEntityListsToViewBag()
        {
            ViewBag.Categories = ViewHelpers.SelectListFromEnum<FileCategory>();
        }

        private bool FileExists(int id)
        {
            return DbContext.Files.Any(e => e.Id == id);
        }
    }
}
