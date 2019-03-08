using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities.JoinEntities
{
    public class OrganizerToContest : PersonToContest
    {
        public string Contribution { get; set; }
    }
}
