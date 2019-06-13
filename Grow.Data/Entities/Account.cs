using Microsoft.AspNetCore.Identity;

namespace Grow.Data.Entities
{
    public class Account : IdentityUser
    {
        public bool IsEnabled { get; set; }
    }
}
