using Grow.Data.Entities;
using Grow.Server.Model.Helpers;
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
    [HtmlTargetElement("search", Attributes = ForAttributeName + "," + ValueAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class PropertySearchTagHelper : BaseTagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string ValueAttributeName = "asp-value";
        private const string CurrentYearOnlyAttributeName = "asp-current-only";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }
        
        [HtmlAttributeName(ValueAttributeName)]
        public ModelExpression ValueExpression { get; set; }

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
            string value, name;
            if (typeof(BaseNamedEntity).IsAssignableFrom(ValueExpression.ModelExplorer.ModelType))
            {
                value = ((BaseNamedEntity)ValueExpression.Model)?.Name;
                name = ValueExpression.ModelExplorer.Metadata.PropertyName + ".Name";
            }
            else
            {
                value = ValueExpression.Model?.ToString();
                name = null;
            }

            var input = new EntitySearchTagHelper(Generator)
            {
                Type = ViewHelpers.GetControllerFor(ValueExpression.ModelExplorer.ModelType),
                Class = Class,
                Style = Style,
                Value = value,
                Name = name,
                ViewContext = ViewContext
            };
            if (CurrentOnly)
            {
                input.FilterName = "year";
                input.FilterValue = ViewContext.ViewBag.SelectedContestYear;
            }

            TagHelperOutput inputOutput = CreateTagHelperOutput("search");
            input.Process(context, inputOutput);

            return inputOutput;
        }
    }
}
