using Microsoft.AspNetCore.Html;
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
    [HtmlTargetElement("search", Attributes = TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class EntitySearchTagHelper : BaseTagHelper
    {
        private const string TypeAttributeName = "asp-type";
        private const string YearAttributeName = "asp-year";

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }

        [HtmlAttributeName(YearAttributeName)]
        public string Year { get; set; }

        public EntitySearchTagHelper(IHtmlGenerator generator) : base(generator, "div", TagMode.StartTagAndEndTag)
        {
        }

        protected override IHtmlContent CreateHtmlContent(TagHelperContext context)
        {
            // check input
            if (ViewContext?.ViewBag?.ContestYears == null)
                throw new ArgumentNullException("ContestYears");

            var contestYears = ViewContext.ViewBag.ContestYears;

            // build tags
            var input = CreateInputElement();
            var list = CreateListElement();
            IHtmlContent script = CreateContestJson(contestYears);

            return WrapInDiv(new[] { input, list, script }, "search-box", "form-group");
        }
        
        private IHtmlContent CreateInputElement()
        {
            var input = new TagBuilder("input");

            // Pass-through attributes to select
            if (Class != null)
                input.Attributes.Add("class", Class);
            if (Style != null)
                input.Attributes.Add("style", Style);

            input.AddCssClass("form-control");
            input.AddCssClass("search-input");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("id", "search-input-" + Id);
            input.Attributes.Add("placeholder", "search by name...");
            input.Attributes.Add("dat-type", Type);
            input.Attributes.Add("dat-output", "search-results-" + Id);

            if (!string.IsNullOrEmpty(Year))
                input.Attributes.Add("dat-year", Year);

            return input;
        }
        
        private IHtmlContent CreateListElement()
        {
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("style", "display: none");
            ul.Attributes.Add("id", "search-results-" + Id);
            return ul;
        }

        private IHtmlContent CreateContestJson(IDictionary<int, string> contestYears)
        {
            var script = new HtmlContentBuilder();
            script.AppendHtmlLine("<script> if (contest_years == undefined) { var contest_years = {");

            foreach (var pair in contestYears)
                script.AppendLine($"{pair.Key}: {pair.Value},");

            script.AppendHtmlLine("}; } </script>");
            return script;
        }
    }
}
