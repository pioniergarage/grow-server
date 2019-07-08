using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.ViewModels
{
    public class AccountViewModel : BaseNamedEntity
    {
        public string CompleteId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public Team Team { get; set; }
        public int? TeamId { get; set; }

        [DisplayName("Can edit content")]
        public bool IsAdmin { get; set; }

        [DisplayName("Can assign admins")]
        public bool IsSuperAdmin { get; set; }
    }
}
