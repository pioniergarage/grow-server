using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grow.Server.Model.TagHelpers
{
    [HtmlTargetElement("li", Attributes = ActivateableAttributeName)]
    public class ActiveNavItemTagHelper : TagHelper
    {
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string ActivateableAttributeName = "conditional-active";

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentController = ViewContext.RouteData.Values["Controller"]?.ToString().ToLower() ?? string.Empty;
            var currentArea = ViewContext.RouteData.Values["Area"]?.ToString().ToLower() ?? string.Empty;

            var isActive = true;
            if (Controller?.Equals(currentController, StringComparison.CurrentCultureIgnoreCase) == false)
                isActive = false;
            if (Area?.Equals(currentArea, StringComparison.CurrentCultureIgnoreCase) == false)
                isActive = false;

            var className = isActive ? "active" : "inactive";
            output.AddClass(className, HtmlEncoder.Default);
            output.Attributes.Remove(output.Attributes.First(a => a.Name.Equals(ActivateableAttributeName)));
        }
    }
}
