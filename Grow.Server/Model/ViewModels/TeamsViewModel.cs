using Grow.Server.Model.Entities;
using System.Collections.Generic;

namespace Grow.Server.Model.ViewModels
{
    public class TeamsViewModel
    {
        public ICollection<Team> Teams { get; set; }
        public ICollection<Prize> Prizes { get; set; }
    }
}
