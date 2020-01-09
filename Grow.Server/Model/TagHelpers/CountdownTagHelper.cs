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
    [HtmlTargetElement("countdown", Attributes = TargetTimeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class CountdownTagHelper : BaseTagHelper
    {
        private const string TargetTimeAttributeName = "asp-datetime";

        [HtmlAttributeName(TargetTimeAttributeName)]
        public DateTime TargetTime { get; set; }

        public CountdownTagHelper(IHtmlGenerator generator) : base(generator, "div", TagMode.StartTagAndEndTag)
        {
        }

        protected override IHtmlContent CreateHtmlContent(TagHelperContext context)
        {
            if (TargetTime < DateTime.Now)
                return null;

            var timeLeft = TargetTime - DateTime.Now;

            var days = CreateSection(timeLeft, CountdownSection.Days);
            var hours = CreateSection(timeLeft, CountdownSection.Hours);
            var mins = CreateSection(timeLeft, CountdownSection.Minutes);
            var secs = CreateSection(timeLeft, CountdownSection.Seconds);

            var wrapper = WrapInDiv(new[] { days, hours, mins, secs }, "clockdiv");
            wrapper.Attributes.Add("dat-datetime", TargetTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));

            return wrapper;
        }
        
        private IHtmlContent CreateSection(TimeSpan timeLeft, CountdownSection section)
        {
            var content = new HtmlContentBuilder();

            var number = new TagBuilder("span");
            number.AddCssClass(section.Identifier);
            number.InnerHtml.Append(section.TimeSelector(timeLeft).ToString(section.FormatString));

            var label = new TagBuilder("div");
            label.AddCssClass("smalltext");
            label.InnerHtml.Append(section.DisplayName);

            return WrapInDiv(new[] { number, label });
        }

        private class CountdownSection
        {
            public static CountdownSection Days = new CountdownSection("days", "Days", d => d.Days, "#00");
            public static CountdownSection Hours = new CountdownSection("hours", "Hrs", d => d.Hours, "00");
            public static CountdownSection Minutes = new CountdownSection("minutes", "Mins", d => d.Minutes, "00");
            public static CountdownSection Seconds = new CountdownSection("seconds", "Secs", d => d.Seconds, "00");

            public readonly string Identifier;
            public readonly string DisplayName;
            public readonly Func<TimeSpan, int> TimeSelector;
            public readonly string FormatString;

            private CountdownSection(string identifier, string displayName, Func<TimeSpan, int> timeSelector, string formatString)
            {
                Identifier = identifier;
                DisplayName = displayName;
                TimeSelector = timeSelector;
                FormatString = formatString;
            }
        }
        
    }
}
