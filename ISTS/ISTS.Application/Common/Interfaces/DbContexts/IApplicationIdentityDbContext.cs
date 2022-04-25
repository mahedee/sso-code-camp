
using ISTS.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISTS.Application.Common.Interfaces.DbContexts
{
    public interface IApplicationIdentityDbContext
    {
        DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
