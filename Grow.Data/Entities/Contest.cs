using System.Collections.Generic;

namespace Grow.Data.Entities
{
    public class Contest : BaseDbEntity
    {
        public string Year { get; set; }
        
        public string RegistrationUrl { get; set; }

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
