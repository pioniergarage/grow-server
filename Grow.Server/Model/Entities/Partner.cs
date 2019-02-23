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

        public virtual Image Image { get; set; }
    }
}
