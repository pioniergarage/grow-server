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
    [HtmlTargetElement("entitysearch", Attributes = TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class EntitySearchTagHelper : TagHelper
    {
        private const string TypeAttributeName = "asp-type";

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }

        public string Class { get; set; }

        public string Style { get; set; }

        public IHtmlGenerator Generator { get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        
        public EntitySearchTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Check input
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            if (ViewContext?.ViewBag?.ContestYears == null)
                throw new ArgumentNullException("ContestYears");

            var contestYears = ViewContext.ViewBag.ContestYears;

            // output config
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            // build tags
            var title = CreateTitleElement();
            var input = CreateInputElement();
            var list = CreateListElement();
            IHtmlContent script = CreateContestJson(contestYears);
            var innerWrapper = WrapInDiv(new[] { input, list, script }, "search-box", "form-group");
            var outerWrapper = WrapInDiv(new[] { title, innerWrapper });

            // Pass-through attributes to select
            if (Class != null)
                outerWrapper.Attributes.Add("class", Class);
            if (Style != null)
                outerWrapper.Attributes.Add("style", Style);

            // set content
            output.Content.SetHtmlContent(outerWrapper);
        }

        private TagBuilder WrapInDiv(IEnumerable<IHtmlContent> elements, string id = null, params string[] classes)
        {
            TagBuilder div = new TagBuilder("div");
            foreach (IHtmlContent element in elements)
            {
                div.InnerHtml.AppendHtml(element);
            }

            if (id != null)
                div.Attributes.Add("id", id);
            if (classes?.Any() == true)
                div.Attributes.Add("class", string.Join(' ', classes));

            return div;
        }

        private IHtmlContent CreateTitleElement()
        {
            var content = new HtmlContentBuilder();

            var title = new TagBuilder("h6");
            title.Attributes.Add("style", "padding-left: 20px");

            content.AppendHtml(title.RenderStartTag());
            content.Append("Copy from old entity");
            content.AppendHtml(title.RenderEndTag());

            return content;
        }

        private IHtmlContent CreateInputElement()
        {
            var input = new TagBuilder("input");
            input.Attributes.Add("type", "texxt");
            input.Attributes.Add("id", "search-input");
            input.Attributes.Add("class", "form-control");
            input.Attributes.Add("placeholder", "search by name...");
            input.Attributes.Add("dat-type", Type);
            input.Attributes.Add("dat-output", "search-results");
            return input;
        }

        private IHtmlContent CreateListElement()
        {
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("style", "display: none");
            ul.Attributes.Add("id", "search-results");
            return ul;
        }

        private IHtmlContent CreateContestJson(IDictionary<int,string> contestYears)
        {
            var script = new HtmlContentBuilder();
            script.AppendHtmlLine("<script> var contest_years = {");

            foreach (var pair in contestYears)
                script.AppendLine($"{pair.Key}: {pair.Value},");

            script.AppendHtmlLine("}; </script>");
            return script;
        }
    }
}
