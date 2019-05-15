namespace Grow.Server.Model.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Ensures that a given text is shortened to stay below a given limit.
        /// The text will be cut off at the last occurance of the characters ' ' or '-' before the limit and will be appended by "...".
        /// If the text is shorter than the given limit then it will be returned without changes.
        /// </summary>
        /// <param name="text">The text that will be adjusted</param>
        /// <param name="limit">The character limit above which the text will be cut</param>
        /// <param name="customSplitChars">A custom char array for splitting can be given to override the default of ' ' and '-'</param>
        /// <returns></returns>
        public static string DynamicSubstring(this string text, int limit, params char[] customSplitChars)
        {
            if (text == null || text.Length <= limit)
                return text;
            var splitChars = customSplitChars.Length > 0 ? customSplitChars : new[] { ' ', '-' };

            var subtext = text.Substring(0, limit);
            var lastSpace = subtext.LastIndexOfAny(splitChars);

            return subtext.Substring(0, lastSpace + 1) + "...";
        }
    }
}
