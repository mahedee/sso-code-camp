using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Core.Entities.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string? RoleDetails { get; set; }
        public bool IsDeletable { get; set; } = true;
    }
}
