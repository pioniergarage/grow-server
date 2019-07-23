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
            var from = options.From ?? DateTime.MinValue;
            var until = options.Until ?? DateTime.MaxValue;
            return date >= from && date <= until;
        }

        public static bool CanUserRegisterNow(this Event evnt, bool isLoggedIn)
        {
            if (evnt == null)
                throw new ArgumentNullException(nameof(evnt));

            var canRegisterInGeneral = evnt.CanVisitorsRegister;
            var isInRegistrationRange = evnt.TeamRegistrationOptions.IsInRange(DateTime.UtcNow);
            var isInFuture = evnt.Start > DateTime.UtcNow;
            var onlyForLoggedInUsers = evnt.Visibility != Event.EventVisibility.Public;

            // TODO: check that logged in user is part of team
            // TODO: check that team is still active if EventVisibility.ActiveTeams

            return canRegisterInGeneral && isInRegistrationRange && isInFuture && (!onlyForLoggedInUsers || isLoggedIn);
        }
    }
}
