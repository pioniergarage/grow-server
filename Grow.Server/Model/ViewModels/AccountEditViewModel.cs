using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.ViewModels
{
    public class AccountEditViewModel : AccountViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
