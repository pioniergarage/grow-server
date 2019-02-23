using Grow.Server.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.ViewModels
{
    public class TeamsViewModel
    {
        public ICollection<Team> Teams { get; set; }
        public ICollection<Prize> Prizes { get; set; }
    }
}
