using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Entities
{
    /// <summary>
    /// Base identity account. Not supposed to be instantiated
    /// </summary>
    public class Account : IdentityUser
    {
    }
}
