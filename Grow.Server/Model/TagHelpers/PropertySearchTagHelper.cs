using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.TagHelpers
{
    [HtmlTargetElement("search", Attributes = ForAttributeName + "," + TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class PropertySearchTagHelper : BaseTagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string TypeAttributeName = "asp-type";
        private const string CurrentYearOnlyAttributeName = "asp-current-only";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }
        
        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }

        [HtmlAttributeName(CurrentYearOnlyAttributeName)]
        public bool CurrentOnly { get; set; }

        public PropertySearchTagHelper(IHtmlGenerator generator) : base(generator, "div", TagMode.StartTagAndEndTag)
        {
        }

        protected override IHtmlContent CreateHtmlContent(TagHelperContext context)
        {
            var hidden = CreateHiddenElement(context);
            var input = CreateSearchElement(context);

            var wrapper = WrapInDiv(new[] { hidden, input }, null, "search-entity-box");
            
            wrapper.Attributes.Add("dat-id", For.Name);

            return wrapper;
        }

        private IHtmlContent CreateHiddenElement(TagHelperContext context)
        {
            var hidden = new InputTagHelper(Generator)
            {
                For = For,
                //InputTypeName = "hidden",
                ViewContext = ViewContext
            };

            var output = CreateTagHelperOutput("input");
            hidden.Process(context, output);

            output.Attributes.SetAttribute("type", "hidden");

            return output;
        }

        private IHtmlContent CreateSearchElement(TagHelperContext context)
        {
            var input = new EntitySearchTagHelper(Generator)
            {
                Type = Type,
                Class = Class,
                Style = Style,
                ViewContext = ViewContext
            };
            if (CurrentOnly)
                input.Year = ViewContext.ViewBag.SelectedContestYear;

            TagHelperOutput inputOutput = CreateTagHelperOutput("search");
            input.Process(context, inputOutput);

            return inputOutput;
        }
    }
}
