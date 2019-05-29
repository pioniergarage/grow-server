using Grow.Data;
using Grow.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Helpers
{
    public static class ViewHelpers
    {
        public static IEnumerable<SelectListItem> SelectListFromEnum<T>()
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Given Type must be an enum");

            var list = new List<SelectListItem>();

            foreach (int value in Enum.GetValues(typeof(T)))
                list.Add(new SelectListItem(Enum.GetName(typeof(T), value), value.ToString()));
            return list;
        }

        public static IEnumerable<SelectListItem> SelectListFromEntities<T>(GrowDbContext context) where T : BaseEntity
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "",
                    Value = null
                }
            };

            foreach (var entity in context.Set<T>())
            {
                var name = entity.Name ?? entity.GetType().Name + " " + entity.Id;
                list.Add(new SelectListItem(name, entity.Id.ToString()));
            }
            return list;
        }

        public static IEnumerable<SelectListItem> SelectListFromEntities<T>(GrowDbContext context, int currentContestId) where T : ContestDependentEntity
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "",
                    Value = null
                }
            };

            foreach (var entity in context.Set<T>().Where(e => e.ContestId == currentContestId))
            {
                var name = entity.Name ?? entity.GetType().Name + " " + entity.Id;
                list.Add(new SelectListItem(name, entity.Id.ToString()));
            }
            return list;
        }
    }
}
