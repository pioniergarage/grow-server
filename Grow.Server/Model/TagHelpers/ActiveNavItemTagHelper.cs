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
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string ActivateableAttributeName = "conditional-active";

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentAction = ViewContext.RouteData.Values["Action"]?.ToString().ToLower() ?? string.Empty;
            var currentController = ViewContext.RouteData.Values["Controller"]?.ToString().ToLower() ?? string.Empty;
            var currentArea = ViewContext.RouteData.Values["Area"]?.ToString().ToLower() ?? string.Empty;

            var isActive = true;
            if (Action?.Equals(currentAction, StringComparison.CurrentCultureIgnoreCase) == false)
                isActive = false;
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
