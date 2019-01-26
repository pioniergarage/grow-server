using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }

        public string JobTitle { get; set; }

        public string Description { get; set; }

        public Image Image { get; set; }

        public string Expertise { get; set; }

        public string Email { get; set; }

        public string LinkedInUrl { get; set; }
    }
}
