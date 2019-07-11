using Grow.Data.Entities;
using Grow.Data.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Team.Model.ViewModels
{
    public class TeamViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [MaxLength(120)]
        public string TagLine { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [FileCategory(FileCategory.TeamLogos)]
        public virtual File LogoImage { get; set; }
        public int? LogoImageId { get; set; }

        [FileCategory(FileCategory.Teams)]
        public virtual File TeamPhoto { get; set; }
        public int? TeamPhotoId { get; set; }

        public virtual Contest Contest { get; set; }
        public int ContestId { get; set; }

        [MaxLength(100)]
        public string ActiveSince { get; set; }

        [Url]
        public string WebsiteUrl { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }
        
        public virtual ICollection<string> Members { get; set; }
    }
}
