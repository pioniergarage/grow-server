using Microsoft.AspNetCore.Mvc.Rendering;

namespace Grow.Server.Model.Utils.Extensions
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
    }
}
