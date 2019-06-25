using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Grow.Data.Entities
{
    public class EventResponse : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        
        public string Email { get; set; }

        public DateTime Created { get; set; }
        
        [DefaultValue(1)]
        [Range(0, 25)]
        public int ParticipantCount { get; set; }
        
        public virtual Event Event { get; set; }
        public int EventId { get; set; }
    }
}
