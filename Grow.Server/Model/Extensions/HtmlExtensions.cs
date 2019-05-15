using Grow.Data.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IHtmlContent ContestSelect(this IHtmlHelper helper, string selectedYear, ICollection<Contest> allContests)
        {
            if (allContests == null)
            {
                return new HtmlString("<i class='text-danger'>Error: Contests not loaded");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<select id='contest-selector' class='form-control'>");

            foreach (var contest in allContests.OrderByDescending(c => c.Year))
            {
                var selectedString = contest.Year.Equals(selectedYear)
                    ? " selected='selected' "
                    : "";
                var contestName = contest.Year.Equals(selectedYear)
                    ? "- " + contest.Name + " -"
                    : contest.Name;
                sb.AppendFormat("<option value='{2}' {0}>{1}</option>", selectedString, contestName, contest.Year).AppendLine();
            }

            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }
    }
}
