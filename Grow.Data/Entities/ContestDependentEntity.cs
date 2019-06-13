using System;
using System.Collections.Generic;
using System.Text;

namespace Grow.Data.Entities
{
    public abstract class ContestDependentEntity : BaseDbEntity
    {

        public virtual Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
