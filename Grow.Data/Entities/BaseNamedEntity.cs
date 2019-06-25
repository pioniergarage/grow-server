using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grow.Data.Entities
{
    public abstract class BaseNamedEntity : BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
