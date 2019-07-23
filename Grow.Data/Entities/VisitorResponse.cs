using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Grow.Data.Entities
{
    public class VisitorResponse : EventResponse
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
