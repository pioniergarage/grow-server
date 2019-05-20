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
    }
}
