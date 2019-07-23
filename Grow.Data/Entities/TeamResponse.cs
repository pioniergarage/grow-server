using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Grow.Data.Entities
{
    public class TeamResponse : EventResponse
    {
        public virtual Team Team { get; set; }
        public int? TeamId { get; set; }

        [Url]
        public string FileUrl { get; set; }
    }
}
