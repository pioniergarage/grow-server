using Grow.Data.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grow.Server.Model.TagHelpers
{
    [HtmlTargetElement("imgselector", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class ImageSelectorTagHelper : TagHelper
    {
        private const string ItemsAttributeName = "asp-items";
        private const string ForAttributeName = "asp-for";
        private const string CategoryAttributeName = "asp-category";
        private const string WrapperClassName = "img-selector";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ItemsAttributeName)]
        public ICollection<SelectListItem> Items { get; set; }

        [HtmlAttributeName(CategoryAttributeName)]
        public string Category { get; set; }
        
        public string Class { get; set; }

        public string Style { get; set; }

        public IHtmlGenerator Generator { get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string Id { get; }

        public ImageSelectorTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
            Id = new Random().Next(1000, 9999).ToString();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Check input
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            // output config
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            // build tags
            var select = CreateSelectElement(context);
            var file = CreateFileElement(context);
            var btn = CreateButtonElement(context);
            var wrapper = WrapInDiv(new[] { select, file, btn });

            // Pass-through attributes to select
            if (Class != null)
                wrapper.Attributes.Add("class", Class);
            if (Style != null)
                wrapper.Attributes.Add("style", Style);

            // Other attributes
            wrapper.AddCssClass(WrapperClassName);
            wrapper.Attributes.Add("name", For.Name);
            wrapper.Attributes.Add("value", For.Model?.ToString() ?? "");

            // set content
            output.Content.SetHtmlContent(wrapper);
        }

        private TagBuilder WrapInDiv(IEnumerable<IHtmlContent> elements)
        {
            TagBuilder div = new TagBuilder("div");
            foreach (IHtmlContent element in elements)
            {
                div.InnerHtml.AppendHtml(element);
            }
            return div;
        }

        private IHtmlContent CreateSelectElement(TagHelperContext context)
        {
            var selectTagHelper = new SelectTagHelper(Generator)
            {
                For = For,
                Items = Items,
                ViewContext = ViewContext,
            };

            TagHelperOutput selectOutput = CreateTagHelperOutput("select");
            selectTagHelper.Process(context, selectOutput);

            selectOutput.AddClass("form-control", HtmlEncoder.Default);
            selectOutput.Attributes.SetAttribute("id", $"file-selector-{Id}");

            return selectOutput;
        }

        private IHtmlContent CreateFileElement(TagHelperContext context)
        {
            var input = new TagBuilder("input");
            input.Attributes.Add("type", "file");
            input.Attributes.Add("id", $"file-upload-{Id}");
            input.Attributes.Add("style", "display: none");
            input.Attributes.Add("dat-category", Category);
            input.Attributes.Add("dat-output", $"file-selector-{Id}");
            return input;
        }

        private IHtmlContent CreateButtonElement(TagHelperContext context)
        {
            var buttonTagHelper = new AnchorTagHelper(Generator)
            {
                Action = "Create",
                Controller = "File",
                RouteValues =
                {
                    { "category", Category }
                },
                ViewContext = ViewContext
            };

            TagHelperOutput buttonOutput = CreateTagHelperOutput("a");
            buttonTagHelper.Process(context, buttonOutput);

            buttonOutput.AddClass("form-control", HtmlEncoder.Default);
            buttonOutput.Attributes.Add("target", "_blank");
            buttonOutput.Content.SetHtmlContent("<i class=\"fa fa-upload\"></i>");

            var fileInputId = "file-upload-" + Id;
            var script = string.Format("document.getElementById('{0}').click(); return false;", fileInputId);
            buttonOutput.Attributes.Add("onclick", script);

            return buttonOutput;
        }

        private TagHelperOutput CreateTagHelperOutput(string tagName)
        {
            return new TagHelperOutput(
                tagName: tagName,
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (_, __) =>
                {
                    return Task.Factory.StartNew<TagHelperContent>(
                            () => new DefaultTagHelperContent());
                }
            );
        }
    }
}
