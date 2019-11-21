using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Grow.Server.Model.Helpers;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Grow.Server.Areas.Admin.Controllers
{
    /// <summary>
    /// Base controller that adds (overridable) CRUD MVC actions for database entities.
    /// 
    /// Only works with entities that contain a contest ID and for which a collection is saved in the Contest class.
    /// 
    /// Navigation property and ViewBag manipulation is done via abstract methods.
    /// </summary>
    /// <typeparam name="T">The entity type for which CRUD actions are created</typeparam>
    public abstract class BaseEntityAdminController<T> : BaseAdminController where T : BaseContestSubEntity
    {
        protected IQueryable<T> SelectedEntities => DbContext.Set<T>().Where(e => e.Contest.Year == SelectedContestYear && GlobalFilter(e));
        protected IQueryable<T> AllEntitiesWithNavProperties => IncludeNavigationProperties(DbContext.Set<T>());
        protected IQueryable<T> SelectedEntitiesWithNavProperties => IncludeNavigationProperties(SelectedEntities);

        protected BaseEntityAdminController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
        }

        /// <summary>
        /// Index action that show a list of all entities.
        /// </summary>
        /// <returns>ICollection<T></returns>
        public virtual async Task<IActionResult> Index()
        {
            return View(await SelectedEntitiesWithNavProperties.ToListAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Details action that show all information about a single entity.
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns>T</returns>
        public virtual async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await AllEntitiesWithNavProperties
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Create action that shows a form for creating a new entity.
        /// </summary>
        /// <returns>null</returns>
        public virtual IActionResult Create()
        {
            AddEntityListsToViewBag();
            return View();
        }
        
        /// <summary>
        /// Create action that receives an entity and saves it to the database and the currently selected contest.
        /// 
        /// Redirects to the Index action when the entity could be created successfully.
        /// </summary>
        /// <param name="entity">New entity</param>
        /// <returns>T, in case of an error</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(T entity)
        {
            RemoveNavigationPropertiesFromModelState<T>();
            if (ModelState.IsValid)
            {
                ViewHelpers.RemoveAllNavigationProperties(entity);

                // Add entity to entity list in contest
                var contest = SelectedContest.Include(EntitiesInContestExpression()).Single();
                var entityList = EntitiesInContestExpression().Compile()(contest);
                entityList.Add(entity);

                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            AddEntityListsToViewBag();
            return View(entity);
        }

        /// <summary>
        /// Edit action that show a form for editing an existing entity.
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns>T</returns>
        public virtual async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await AllEntitiesWithNavProperties.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (entity == null)
            {
                return NotFound();
            }

            AddEntityListsToViewBag();
            return View(entity);
        }

        /// <summary>
        /// Edit action that receives an entity and updates its values in the database.
        /// 
        /// Redirects to the Index action when the entity could be edited successfully.
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <param name="entity">New version of the entity</param>
        /// <returns>T, in case of an error</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(int id, T entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            RemoveNavigationPropertiesFromModelState<T>();
            if (ModelState.IsValid)
            {
                ViewHelpers.RemoveAllNavigationProperties(entity);
                try
                {
                    DbContext.Update(entity);
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.Id))
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
            return View(entity);
        }

        /// <summary>
        /// Delete action that shows details of an entity and asks for confirmation to delete it
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns>T</returns>
        public virtual async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await AllEntitiesWithNavProperties
                .FirstOrDefaultAsync(m => m.Id == id)
                .ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        /// <summary>
        /// Delete action that deletes a given entity from the database.
        /// 
        /// Redirects to the Index action afterwards. No view needed.
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await DbContext.Set<T>().FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Changes the IsActive property of a given entity.
        /// 
        /// Redirects to the Index action afterwards. No view needed.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        /// <param name="value">New value for the IsActive property</param>
        /// <returns></returns>
        public virtual async Task<IActionResult> Toggle(int id, bool value)
        {
            var entity = await DbContext.Set<T>().FindAsync(id).ConfigureAwait(false);
            entity.IsActive = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        protected bool EntityExists(int id)
        {
            return SelectedEntities.Any(e => e.Id == id);
        }

        /// <summary>
        /// Prototype function that returns an expression for accessing an ICollection<T> property in the Contest class.
        /// 
        /// Is used to make changes to the entity collection in the current contest.
        /// </summary>
        /// <returns>Property accessor expression</returns>
        protected abstract Expression<Func<Contest, ICollection<T>>> EntitiesInContestExpression();

        /// <summary>
        /// Prototype function that can be used to fill the ViewBag with data necessary in views displaying entity details.
        /// </summary>
        protected abstract void AddEntityListsToViewBag();

        /// <summary>
        /// Prototype function that can add Include() statements for all navigation properties necessary in the views.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract IQueryable<T> IncludeNavigationProperties(IQueryable<T> query);
    }
}
