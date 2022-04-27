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
    public class RolePermissionsRepository : IRolePermissionsRepository
    {
        private readonly ApplicationDbContext _context;

        public RolePermissionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermissions>> GetRolePermissions(string roleId, int apiResourceId)
        {
            return await _context.RolePermissions.Include(x => x.Permission)
                .Include(x => x.Role).Where(x => x.RoleId == roleId && x.ApiResourceId == apiResourceId).ToListAsync();
        }

        public async Task<IEnumerable<RolePermissions>> GetRolePermissions(string roleId)
        {
            return await _context.RolePermissions.Include(x => x.Permission)
                .Include(x => x.Role).Where(x => x.RoleId == roleId).ToListAsync();
        }

        public async Task<RolePermissions> GetRolePermissionDetails(string roleId, int permissionId)
        {
            return await _context.RolePermissions.Include(x => x.Permission)
                .Include(x => x.Role).Where(x => x.RoleId == roleId && x.Id == permissionId).FirstOrDefaultAsync();
        }

        public async Task<RolePermissions> CreateRolePermissions(RolePermissions rolePermissions)
        {
            await _context.RolePermissions.AddAsync(rolePermissions);
            await _context.SaveChangesAsync();
            return rolePermissions;
        }

        public async Task<int> CreateRolePermissions(IEnumerable<RolePermissions> rolePermissions)
        {
            var rolePermissionsEnumerable = rolePermissions.ToList();
            await _context.RolePermissions.AddRangeAsync(rolePermissionsEnumerable);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CreateRolePermissions(IEnumerable<RolePermissions> rolePermissions, string roleId, int apiResourceId)
        {
            var existingRolePermissions = await _context.RolePermissions
                .Where(x => x.RoleId == roleId && x.ApiResourceId == apiResourceId).AsNoTracking().ToListAsync();

            _context.RolePermissions.RemoveRange(existingRolePermissions);

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            return await _context.SaveChangesAsync();
        }

        public async Task<RolePermissions> DeleteRolePermissions(RolePermissions permission)
        {
            _context.RolePermissions.Remove(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<int> DeleteRolePermissions(IEnumerable<RolePermissions> permission)
        {
            _context.RolePermissions.RemoveRange(permission);
            return await _context.SaveChangesAsync();
        }
    }
}
