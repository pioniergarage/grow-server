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
        [Range(0, 10, ErrorMessage = "You can only select a number between 0 and 10")]
        [DisplayName("How many are coming?")]
        public int ParticipantCount { get; set; }

        public virtual Event Event { get; set; }
        public int EventId { get; set; }

        [DisplayName("Team name")]
        public string TeamName { get; set; }
        
        [DisplayName("File to submit")]
        public IFormFile UploadedFile { get; set; }

        [DisplayName("File to submit (External)")]
        [Url]
        public string ExternalFileUrl { get; set; }

        public TeamResponseViewModel()
        {
        }

        public TeamResponseViewModel(TeamResponse response)
        {
            ParticipantCount = response.ParticipantCount;
            Event = response.Event;
            EventId = response.EventId;
            TeamName = response.Team?.Name;
            ExternalFileUrl = response.FileUrl;
        }
    }
}
