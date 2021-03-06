﻿using Grow.Data.Entities;
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
        
        [DisplayName("Team Response")]
        public int? TeamResponse { get; set; }

        [DisplayName("Can the team respond right now?")]
        public bool CanTeamRespondNow { get; set; }

        public TeamEventViewModel() : base()
        {
        }

        public TeamEventViewModel(Event evnt, Team team) : base(evnt)
        {
            SlidesUrl = evnt.Slides?.Url;
            TeamResponse = evnt.Responses
                ?.Where(r => r is TeamResponse tr && tr.TeamId == team.Id)
                .SingleOrDefault()
                ?.ParticipantCount;
            CanTeamRespondNow = evnt.CanTeamRespondNow(team);
        }
    }
}
