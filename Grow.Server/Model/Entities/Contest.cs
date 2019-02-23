using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model.Entities.JoinEntities;

namespace Grow.Server.Model.Entities
{
    public class Contest : BaseEntity
    {
        public string Name { get; set; }

        public virtual Event KickoffEvent { get; set; }

        public virtual Event FinalEvent { get; set; }

        public string Language { get; set; }


        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<PartnerToContest> Partners { get; set; }

        public virtual ICollection<OrganizerToContest> Organizers { get; set; }

        public virtual ICollection<MentorToContest> Mentors { get; set; }

        public virtual ICollection<JudgeToContest> Judges { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
