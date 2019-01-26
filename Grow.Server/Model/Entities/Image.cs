using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public string AltText { get; set; }
    }
}
