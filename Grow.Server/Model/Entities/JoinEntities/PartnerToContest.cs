using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities.JoinEntities
{
    public class PartnerToContest
    {
        public Partner Partner { get; set; }
        public int PartnerId { get; set; }

        public Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
