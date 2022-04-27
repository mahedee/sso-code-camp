using ISTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IRolePermissionsRepository
    {
        Task<IEnumerable<RolePermissions>> GetRolePermissions(string roleId, int apiResourceId);
        Task<IEnumerable<RolePermissions>> GetRolePermissions(string roleId);
        Task<RolePermissions> GetRolePermissionDetails(string roleId, int permissionId);
        Task<RolePermissions> CreateRolePermissions(RolePermissions rolePermissions);
        Task<int> CreateRolePermissions(IEnumerable<RolePermissions> rolePermissions);
        Task<int> CreateRolePermissions(IEnumerable<RolePermissions> rolePermissions, string roleId, int apiResourceId);
        Task<RolePermissions> DeleteRolePermissions(RolePermissions permission);
        Task<int> DeleteRolePermissions(IEnumerable<RolePermissions> permission);
    }
}
