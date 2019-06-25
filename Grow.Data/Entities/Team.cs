using Grow.Data.Helpers.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grow.Data.Entities
{
    public class Team : BaseContestSubEntity
    {
        public string TagLine { get; set; }

        public string Description { get; set; }

        [FileCategory(FileCategory.TeamLogos)]
        public virtual File LogoImage { get; set; }
        public int? LogoImageId { get; set; }

        [FileCategory(FileCategory.Teams)]
        public virtual File TeamPhoto { get; set; }
        public int? TeamPhotoId { get; set; }

        public string ActiveSince { get; set; }

        public string WebsiteUrl { get; set; }

        public string Email { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }

        public bool HasDroppedOut { get; set; }

        public string MembersAsString
        {
            get => string.Join(", ", Members);
            set => Members = value.Split(", ");
        }

        [NotMapped]
        public virtual ICollection<string> Members { get; set; }
    }
}
