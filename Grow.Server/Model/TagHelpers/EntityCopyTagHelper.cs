using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.TagHelpers
{
    [HtmlTargetElement("copyentity", Attributes = TypeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class EntityCopyTagHelper : BaseTagHelper
    {
        private const string TypeAttributeName = "asp-type";

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }
        
        public EntityCopyTagHelper(IHtmlGenerator generator) : base(generator, "div", TagMode.StartTagAndEndTag)
        {
        }

        protected override IHtmlContent CreateHtmlContent(TagHelperContext context)
        {
            var title = CreateTitleElement();
            var input = CreateSearchElement(context);

            return WrapInDiv(new[] { title, input }, "copy-entity-box");
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

        private IHtmlContent CreateSearchElement(TagHelperContext context)
        {
            var input = new EntitySearchTagHelper(Generator)
            {
                Type = Type,
                Class = Class,
                Style = Style,
                ViewContext = ViewContext
            };

            TagHelperOutput inputOutput = CreateTagHelperOutput("search");
            input.Process(context, inputOutput);

            return inputOutput;
        }
    }
}
