using Microsoft.AspNetCore.Identity;

namespace ISTS.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeletable { get; set; } = true;
    }
}
