using Grow.Data.Entities;
using Grow.Data.Helpers.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grow.Server.Model.TagHelpers
{
    [HtmlTargetElement("upload", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class FileUploadTagHelper : BaseTagHelper
    {
        private const string EnableSearchAttributeName = "asp-enable-search";
        private const string ForAttributeName = "asp-for";
        private const string CategoryAttributeName = "asp-prop";
        private const string WrapperClassName = "file-uploader";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(EnableSearchAttributeName)]
        public bool IsSearchEnabled { get; set; }

        [HtmlAttributeName(CategoryAttributeName)]
        public ModelExpression FileProperty { get; set; }
        private string CategoryString
        {
            get
            {
                var category = FileCategory.Misc;
                var attr = FileProperty?.Metadata.ContainerType.GetProperty(FileProperty.Name).GetCustomAttribute<FileCategoryAttribute>();
                if (attr != null)
                    category = attr.Category;

                return category.ToString().ToLower();
            }
        }
        
        public FileUploadTagHelper(IHtmlGenerator generator) 
            : base(generator, "div", TagMode.StartTagAndEndTag)
        {
        }
        
        protected override IHtmlContent CreateHtmlContent(TagHelperContext context)
        {
            // build tags
            var input = CreateInputElement(context);
            var file = CreateFileElement(context);
            var btn = CreateButtonElement(context);
            var pre = CreatePreviewElement(context);
            var wrapper = WrapInDiv(new[] { input, file, btn, pre });
            
            // Other attributes
            wrapper.AddCssClass(WrapperClassName);

            // set content
            return wrapper;
        }

        private IHtmlContent CreateInputElement(TagHelperContext context)
        {
            // Create combination of two inputs, one visible for file name, one hidden for file id

            // Create name input
            IHtmlContent nameInputContent;
            if (IsSearchEnabled)
            {
                var nameInputTagHelper = new EntitySearchTagHelper(Generator)
                {
                    Type = typeof(File).Name,
                    Value = (FileProperty.Model as File)?.Name,
                    Class = "file-name-input",
                    FilterName = "category",
                    FilterValue = CategoryString,
                    ViewContext = ViewContext
                };
                var nameInputOutput = CreateTagHelperOutput("search");
                nameInputTagHelper.Process(context, nameInputOutput);
                nameInputContent = nameInputOutput;
            }
            else
            {
                var nameInput = new TagBuilder("input");
                nameInput.Attributes.Add("type", "text");
                nameInput.Attributes.Add("class", "form-control file-name-input");
                nameInput.Attributes.Add("readonly", "readonly");
                nameInput.Attributes.Add("value", (FileProperty.Model as File)?.Name);
                nameInputContent = nameInput;
            }

            // create id input
            var idInputHelper = new InputTagHelper(Generator)
            {
                For = For,
                ViewContext = ViewContext
            };
            var idInputContent = CreateTagHelperOutput("input");
            idInputHelper.Process(context, idInputContent);
            idInputContent.Attributes.Add("class", "file-id-input");
            idInputContent.Attributes.SetAttribute("type", "hidden");
            idInputContent.Attributes.Add("value", For.Model?.ToString());

            // return input combination
            return WrapInDiv(new[] { nameInputContent, idInputContent });
        }

        private IHtmlContent CreateFileElement(TagHelperContext context)
        {
            var input = new TagBuilder("input");
            input.Attributes.Add("type", "file");
            input.Attributes.Add("id", $"file-upload-{Id}");
            input.Attributes.Add("style", "display: none");
            input.Attributes.Add("dat-category", CategoryString);
            input.Attributes.Add("dat-output", For.Name);
            return input;
        }

        private IHtmlContent CreatePreviewElement(TagHelperContext context)
        {
            var input = new TagBuilder("img");
            input.Attributes.Add("id", $"file-preview-{Id}");
            input.Attributes.Add("style", "display: none");
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
                    { "category", CategoryString }
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
    }
}
