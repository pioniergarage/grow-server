using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grow.Data.Entities
{
    public class File : BaseTimestampedEntity
    {
        [Required]
        public string Url { get; set; }

        public string AltText { get; set; }

        public string Extension { get; set; }

        public string Category { get; set; }
    }

    public enum FileCategory
    {
        Misc,
        Events,
        Partners,
        People,
        Teams,
        TeamLogos,
        Slides
    }
}
