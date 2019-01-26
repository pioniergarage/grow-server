using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities.JoinEntities
{
    public class OrganizerToContest
    {
        public Person Person { get; set; }
        public int PersonId { get; set; }

        public Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
