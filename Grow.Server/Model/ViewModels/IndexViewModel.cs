using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.ViewModels
{
    public class IndexViewModel
    {
        public Contest Contest { get; set; }
        public ICollection<Event> MainEvents { get; set; }
        public ICollection<Partner> Partners { get; set; }
    }
}
