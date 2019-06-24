using Grow.Data;
using Grow.Data.Entities;
using Grow.Data.Helpers.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public static IEnumerable<SelectListItem> SelectListFromEntities<T>(GrowDbContext context) where T : BaseDbEntity
        {
            var files = context
                .Set<T>()
                .OrderBy(e => e.Name);

            return SelectListFromEntityList(files);
        }

        public static IEnumerable<SelectListItem> SelectListFromEntities<T>(GrowDbContext context, int currentContestId) where T : ContestDependentEntity
        {
            var files = context
                .Set<T>()
                .Where(e => e.ContestId == currentContestId)
                .OrderBy(e => e.Name);

            return SelectListFromEntityList(files);
        }
        
        public static IEnumerable<SelectListItem> SelectListFromFiles<TSource>(GrowDbContext context, Expression<Func<TSource, File>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("Invalid referenced member", nameof(propertyLambda));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException("Referenced member is not a property", nameof(propertyLambda));

            var fileCategory = FileCategory.Misc;
            var attr = propInfo.GetCustomAttribute<FileCategoryAttribute>();
            if (attr != null)
                fileCategory = attr.Category;

            var files = context
                .Set<File>()
                .Where(e => (e.Category ?? "misc").Equals(fileCategory.ToString(), StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(e => e.Name);

            return SelectListFromEntityList(files);
        }

        private static IEnumerable<SelectListItem> SelectListFromEntityList(IEnumerable<BaseEntity> entities)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "",
                    Value = null
                }
            };

            foreach (var entity in entities)
            {
                var name = entity.Name ?? entity.GetType().Name + " " + entity.Id;
                var item = new SelectListItem(name, entity.Id.ToString());
                list.Add(item);
            }

            return list;
        }
    }
}
