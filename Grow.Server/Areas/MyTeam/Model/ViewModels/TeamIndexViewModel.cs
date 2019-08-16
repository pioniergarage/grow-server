using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.MyTeam.Model.ViewModels
{
    public class TeamIndexViewModel
    {
        public TeamViewModel MyTeam { get; set; }
        public IEnumerable<TeamEventViewModel> UpcomingEvents { get; set; }
    }
}
