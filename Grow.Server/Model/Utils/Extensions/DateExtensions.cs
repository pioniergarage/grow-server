using System;

namespace Grow.Server.Model.Utils.Extensions
{
    public static class DateExtensions
    {
        public static string GetDayExtension(this DateTime dt)
        {
            return (dt.Day % 10 == 1 && dt.Day != 11) ? "st"
                : (dt.Day % 10 == 2 && dt.Day != 12) ? "nd"
                : (dt.Day % 10 == 3 && dt.Day != 13) ? "rd"
                : "th";
        }

        public static int TotalMonths(this DateTime dt)
        {
            return dt.Year * 12 + dt.Month;
        }
    }
}
