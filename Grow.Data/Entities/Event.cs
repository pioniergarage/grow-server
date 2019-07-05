using Grow.Data.Helpers.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Data.Entities
{
    public class Event : BaseContestSubEntity
    {
        /* Value properties */

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Url]
        public string ExternalEventUrl { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        public EventVisibility Visibility { get; set; }

        public EventType Type { get; set; }

        public bool HasTimesSet { get; set; }

        public bool IsMandatory { get; set; }

        /* Navigation Properties */

        public RegistrationOptions RegistrationOptions { get; set; }

        [FileCategory(FileCategory.Events)]
        public virtual File Image { get; set; }
        public int? ImageId { get; set; }

        [FileCategory(FileCategory.Slides)]
        public virtual File Slides { get; set; }
        public int? SlidesId { get; set; }

        public virtual Partner HeldBy { get; set; }
        public int? HeldById { get; set; }

        public virtual ICollection<EventResponse> Responses { get; set; }


        public enum EventVisibility
        {
            Public,
            ForAllTeams,
            ForActiveTeams
        }

        public enum EventType
        {
            Other,
            MainEvent,
            Workshop,
            Mentoring
        }

        public enum RegistrationType
        {
            None,
            Optional,
            Mandatory
        }
    }

    [Owned]
    public class RegistrationOptions
    {
        public Event.RegistrationType Type { get; set; }

        public DateTime? From { get; set; }

        public DateTime? Until { get; set; }
    }
}
