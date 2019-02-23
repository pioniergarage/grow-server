using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }

        public string TagLine { get; set; }

        public string Description { get; set; }

        public virtual Image LogoImage { get; set; }

        public virtual Image TeamPhoto { get; set; }

        public string ActiveSince { get; set; }

        public string WebsiteUrl { get; set; }

        public string Email { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }

        public bool IsActive { get; set; }

        public Contest Contest { get; set; }
        
        public string MembersAsString
        {
            get => string.Join(", ", Members);
            set => Members = value.Split(", ");
        }

        [NotMapped]
        public virtual ICollection<string> Members { get; set; }
    }
}
