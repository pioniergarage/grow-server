using System.Collections.Generic;

namespace Grow.Data.Entities
{
    public class Contest : BaseEntity
    {
        public string Name { get; set; }

        public string Year { get; set; }

        public virtual Event KickoffEvent { get; set; }

        public virtual Event FinalEvent { get; set; }

        public string Language { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }

        public virtual ICollection<Organizer> Organizers { get; set; }

        public virtual ICollection<Mentor> Mentors { get; set; }

        public virtual ICollection<Judge> Judges { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Prize> Prizes { get; set; }
    }
}
