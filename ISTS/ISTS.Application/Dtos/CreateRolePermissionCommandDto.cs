using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Dtos
{
    public record CreateRolePermissionCommandDto
    {
        public CreateRolePermissionCommandDto(string roleId, int permissionId, int apiResourceId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            ApiResourceId = apiResourceId;
        }

        public string RoleId { get; private set; }
        public int PermissionId { get; private set; }
        public int ApiResourceId { get; private set; }
    }
}