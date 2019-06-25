using Grow.Data.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grow.Data.Entities
{
    public class Event : BaseContestSubEntity
    {
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Url]
        public string ExternalEventUrl { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        [FileCategory(FileCategory.Events)]
        public virtual File Image { get; set; }
        public int? ImageId { get; set; }

        public EventVisibility Visibility { get; set; }

        public EventType Type { get; set; }

        public EventRegistration Registration { get; set; }

        public bool HasTimesSet { get; set; }

        public bool IsMandatory { get; set; }

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

        public enum EventRegistration
        {
            None,
            Optional,
            Mandatory
        }
    }
}
