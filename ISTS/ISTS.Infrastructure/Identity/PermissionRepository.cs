using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities;
using ISTS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure.Identity
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByApiResourceId(int clientId)
        {
            return await _context.Permissions.Where(x => x.ApiResourceId == clientId).ToListAsync();
        }

        public async Task<Permission?> GetPermissionDetails(int permissionId)
        {
            return await _context.Permissions.FirstOrDefaultAsync(x => x.Id == permissionId);
        }

        public async Task<Permission?> GetPermissionDetails(string permissionName, int apiResourceId)
        {
            return await _context.Permissions.FirstOrDefaultAsync(x => x.Name == permissionName && x.ApiResourceId == apiResourceId);
        }

        public async Task<bool> IsPermissionExists(Permission permission)
        {
            return await _context.Permissions.AnyAsync(x => x.Name.ToLower() == permission.Name.ToLower() && x.ApiResourceId == permission.ApiResourceId);
        }

        public async Task<Permission> CreatePermission(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();

            return permission;
        }

        public async Task DeletePermission(int permissionId)
        {
            var permissionDetails = await GetPermissionDetails(permissionId);
            if (permissionDetails == null)
            {
                throw new NotFoundException("Permission information not found");
            }

            var roleClaims = await _context.RoleClaims.Where(x => x.ClaimType == permissionDetails.Name).ToListAsync();

            if (roleClaims.Any())
            {
                _context.RoleClaims.RemoveRange(roleClaims);
            }

            _context.Permissions.Remove(permissionDetails);
            await _context.SaveChangesAsync();
        }
    }
}
