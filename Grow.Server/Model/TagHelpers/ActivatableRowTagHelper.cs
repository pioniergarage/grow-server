using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grow.Server.Model.TagHelpers
{
    [HtmlTargetElement("tr", Attributes = IsActiveAttributeName)]
    public class ActivatableRowTagHelper : TagHelper
    {
        private const string IsActiveAttributeName = "is-active";

        [HtmlAttributeName(IsActiveAttributeName)]
        public string IsActive { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var className = bool.Parse(IsActive) ? "active" : "inactive";
            output.AddClass(className, HtmlEncoder.Default);
        }
    }
}
