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
using Grow.Server.Model.Entities.JoinEntities;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrganizersController : BaseAdminController
    {
        public OrganizersController(GrowDbContext dbContext, IOptions<AppSettings> appSettings) : base(dbContext, appSettings)
        {
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await OrganizersInSelectedYear.ToListAsync().ConfigureAwait(false));
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,JobTitle,Description,Expertise,Email,WebsiteUrl,Id")] Person person)
        {
            if (ModelState.IsValid)
            {
                var link = new OrganizerToContest
                {
                    Contest = SelectedContest.Single(),
                    Person = person
                };
                SelectedContest.Include(c => c.Organizers).Single().Organizers.Add(link);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
    }
}
