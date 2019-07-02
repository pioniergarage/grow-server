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
    public abstract class BaseTagHelper : TagHelper
    {
        public string Class { get; set; }

        public string Style { get; set; }

        public string Id { get; }

        public IHtmlGenerator Generator { get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string TagName { get; }

        public TagMode TagMode { get; }

        protected BaseTagHelper(IHtmlGenerator generator, string tagName, TagMode tagMode)
        {
            Generator = generator;
            Id = new Random().Next(1000, 9999).ToString();
            TagName = tagName;
            TagMode = tagMode;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Check input
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            // output config
            output.TagName = TagName;
            output.TagMode = TagMode;

            IHtmlContent content = CreateHtmlContent(context);

            // set content
            output.Content.SetHtmlContent(content);
        }

        protected abstract IHtmlContent CreateHtmlContent(TagHelperContext context);

        protected static TagBuilder WrapInDiv(IEnumerable<IHtmlContent> elements, string id = null, params string[] classes)
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

        protected static TagHelperOutput CreateTagHelperOutput(string tagName)
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
