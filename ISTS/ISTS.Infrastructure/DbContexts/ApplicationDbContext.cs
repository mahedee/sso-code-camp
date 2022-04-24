using ISTS.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationUserRoleClaim, ApplicationUserToken>
    {

    }
}
