using Grow.Data.Entities;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Grow.Server.Model.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Returns a given string value if it is neither null nor an empty string. A default value can be given that will be returned instead in that case.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value"></param>
        /// <param name="textIfEmpty"></param>
        /// <returns></returns>
        public static string PrintIfNonEmpty(this IHtmlHelper helper, string value, string textIfEmpty = "")
        {
            if (string.IsNullOrEmpty(value))
                return textIfEmpty;

            return value;
        }

        public static string GetController<TEntity>(this IHtmlHelper<TEntity> html) 
            where TEntity : BaseEntity
        {
            return typeof(TEntity).Name.ToLower() + "s";
        }

        public static string GetControllerFor<TEntity,TProperty>(this IHtmlHelper<TEntity> html, Expression<Func<TEntity,TProperty>> expression) 
            where TEntity : BaseEntity where TProperty : BaseEntity
        {
            return ViewHelpers.GetControllerFor(typeof(TProperty));
        }

        public static IHtmlContent PaginationLinks(this IHtmlHelper helper)
        {
            var htmlString = string.Format(
                "<div class=\"pagination\" current=\"{0}\" max=\"{1}\"></div>",
                helper.ViewBag.CurrentPage ?? 1,
                helper.ViewBag.PageCount ?? 1
            );
            return new HtmlString(htmlString);
        }

        public static IHtmlContent ContestSelector(this IHtmlHelper helper)
        {
            string selectedYear = helper.ViewBag.SelectedContestYear;
            IDictionary<int, string> contestYears = helper.ViewBag.ContestYears;
            if (contestYears == null)
            {
                return new HtmlString("<i class='text-danger'>Error: Contests not loaded");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<select id='contest-selector'>");

            foreach (var contestYear in contestYears.Values.OrderByDescending(c => c))
            {
                var selectedString = contestYear.Equals(selectedYear)
                    ? " selected='selected' "
                    : "";
                var contestName = "GROW " + contestYear;
                sb.AppendFormat("<option value='{2}' {0}>{1}</option>", selectedString, contestName, contestYear).AppendLine();
            }

            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }
    }
}
