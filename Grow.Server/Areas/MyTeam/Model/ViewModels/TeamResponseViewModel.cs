using Grow.Data.Entities;
using Grow.Server.Model.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.MyTeam.Model.ViewModels
{
    public class TeamResponseViewModel
    {
        [DefaultValue(1)]
        [Range(0, 25)]
        [DisplayName("How many are coming?")]
        public int ParticipantCount { get; set; }

        public virtual Event Event { get; set; }
        public int EventId { get; set; }

        [DisplayName("Team name")]
        public string TeamName { get; set; }
        
        [DisplayName("File to submit")]
        public IFormFile File { get; set; }
    }
}
