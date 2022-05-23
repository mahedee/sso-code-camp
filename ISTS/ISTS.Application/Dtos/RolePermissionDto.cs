using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTS.Core.Entities;
using ISTS.Core.Entities.Identity;

namespace ISTS.Application.Dtos
{
    public class RolePermissionDto
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
        public int ApiResourceId { get; set; }
        public string ApiResourceName { get; set; }
        public string ApiResourceDisplayName { get; set; }
        public RoleDto Role { get; set; }
        public PermissionDto Permission { get; set; }
    }
}
