using System;

namespace Grow.Data.Entities
{
    public class Event : ContestDependentEntity
    {
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string FacebookLink { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        public virtual Image Image { get; set; }
        public int? ImageId { get; set; }

        public EventVisibility Visibility { get; set; }

        public EventType Type { get; set; }

        public bool HasTimesSet { get; set; }

        public bool IsMandatory { get; set; }

        public virtual Partner HeldBy { get; set; }
        public int? HeldById { get; set; }


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
    }
}
