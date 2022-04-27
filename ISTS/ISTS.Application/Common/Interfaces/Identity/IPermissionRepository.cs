using ISTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetPermissionsByApiResourceId(int clientId);
        Task<Permission?> GetPermissionDetails(int permissionId);
        Task<Permission?> GetPermissionDetails(string permissionName, int apiResourceId);
        Task<bool> IsPermissionExists(Permission permission);
        Task<Permission> CreatePermission(Permission permission);
        Task DeletePermission(int permissionId);
    }
}
