using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsInRange(this TeamRegistrationOptions options,DateTime date)
        {
            var from = options?.From ?? DateTime.MinValue;
            var until = options?.Until ?? DateTime.MaxValue;
            return date >= from && date <= until;
        }

        public static bool CanVisitorRespondNow(this Event evnt)
        {
            if (evnt == null)
                throw new ArgumentNullException(nameof(evnt));

            if (!evnt.CanVisitorsRegister)
                return false;

            if (evnt.Start <= DateTime.UtcNow)
                return false;

            if (evnt.Visibility != Event.EventVisibility.Public)
                return false;

            return true;
        }

        public static bool CanTeamRespondNow(this Event evnt, Team team)
        {
            if (evnt == null)
                throw new ArgumentNullException(nameof(evnt));
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            if (!evnt.CanTeamsRegister)
                return false;

            if (evnt.Start <= DateTime.UtcNow)
                return false;

            if (!evnt.TeamRegistrationOptions.IsInRange(DateTime.Now))
                return false;

            if (evnt.ContestId != team.ContestId)
                return false;

            if (evnt.Visibility == Event.EventVisibility.ForActiveTeams && team.HasDroppedOut)
                return false;

            return true;
        }
    }
}
