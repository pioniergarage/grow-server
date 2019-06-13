using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Models.ViewModels
{
    public class AccountIndexViewModel : AccountViewModel
    {
        public bool CanEdit { get; set; }
    }
}
