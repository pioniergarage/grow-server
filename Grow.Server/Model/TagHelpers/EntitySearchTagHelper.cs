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
    /// <summary>
    /// Taghelper that creates a search bar with a search result popup to search for entities by name
    /// </summary>
    [HtmlTargetElement("search", Attributes = TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class EntitySearchTagHelper : BaseTagHelper
    {
        private const string TypeAttributeName = "asp-type";
        private const string FilterNameAttributeName = "asp-filter-name";
        private const string FilterValueAttributeName = "asp-filter-value";

        /// <summary>
        /// Type name of the entities that selects the search table. Must fit a type tracked by the database context
        /// </summary>
        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }

        /// <summary>
        /// Limit the search by adding a filter that is applied on the REST call. Set together with FilterValue.
        /// </summary>
        [HtmlAttributeName(FilterNameAttributeName)]
        public string FilterName { get; set; }

        /// <summary>
        /// Limit the search by adding a filter that is applied on the REST call. Set together with FilterName.
        /// </summary>
        [HtmlAttributeName(FilterValueAttributeName)]
        public string FilterValue { get; set; }

        /// <summary>
        /// Allows for a value to be pre-selected in the search field
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Assign a name to the search bar input
        /// </summary>
        public string Name { get; set; }

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

            return WrapInDiv(new[] { input, list, script }, "search-box");
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
            if (Value != null)
                input.Attributes.Add("value", Value);
            if (Name != null)
                input.Attributes.Add("name", Name);
            input.Attributes.Add("id", "search-input-" + Id);
            input.Attributes.Add("placeholder", "search by name...");
            input.Attributes.Add("dat-type", Type);
            input.Attributes.Add("dat-output", "search-results-" + Id);

            if (!string.IsNullOrEmpty(FilterValue) && !string.IsNullOrEmpty(FilterName))
            {
                input.Attributes.Add("dat-filter-name", FilterName);
                input.Attributes.Add("dat-filter-value", FilterValue);
            }

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
