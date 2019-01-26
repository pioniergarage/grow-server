using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities
{
    public class Partner : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Contribution { get; set; }

        public Image Image { get; set; }
    }
}
