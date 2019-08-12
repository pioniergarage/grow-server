using Grow.Data.Entities;
using Grow.Server.Model.Extensions;
using Grow.Server.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.MyTeam.Model.ViewModels
{
    public class TeamEventViewModel : EventViewModel
    {
        [DisplayName("Link to the slides")]
        public string SlidesUrl { get; set; }
        
        [DisplayName("Responded to event?")]
        public bool HasResponded { get; set; }

        [DisplayName("Can the team respond right now?")]
        public bool CanTeamRespondNow { get; set; }

        public TeamEventViewModel(Event evnt, Team team) : base(evnt)
        {
            SlidesUrl = evnt.Slides?.Url;
            HasResponded = evnt.Responses.Any(r => r is TeamResponse tr && tr.TeamId == team.Id);
            CanTeamRespondNow = evnt.CanTeamRespondNow(team);
        }
    }
}
