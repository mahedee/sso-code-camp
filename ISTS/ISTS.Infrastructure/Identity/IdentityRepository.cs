using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Extensions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using ISTS.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure.Identity
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public IdentityRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<bool> ExistsUserAsync(string userId)
        {
            return await _userManager.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<bool> ExistsRoleAsync(string roleId)
        {
            return await _roleManager.Roles.AnyAsync(x => x.Id == roleId);
        }

        public async Task<bool> IsUserInRoleAsync(string role, string userId)
        {
            return await _userManager.IsInRoleAsync(await GetUserAsync(userId), role);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            return await _userManager.Users
                .WhereIf(!string.IsNullOrEmpty(search), x => x.UserName.ToLower().Contains(search))
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return await _userManager.GetUsersInRoleAsync(role.Name);
        }

        public async Task<IEnumerable<ApplicationUser>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10)
        {
            var users = await _context.Users.Join(_context.UserClaims, u => u.Id,
                    uc => uc.UserId, (u, uc) => new { u, uc })
                .Where(x => x.uc.ClaimType.Equals(claimType) && x.uc.ClaimValue.Equals(claimValue))
                .Select(t => t.u).Distinct()
                .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesAsync(string search, int page = 1, int pageSize = 10)
        {
            return await _roleManager.Roles.WhereIf(!string.IsNullOrEmpty(search), x => x.Name.ToLower().Contains(search.ToLower())).ToListAsync();
        }

        public async Task<(IdentityResult identityResult, string roleId)> CreateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return (result, role.Id);
        }

        public async Task<ApplicationRole> GetRoleAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<List<ApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return (result, role.Id);
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<(IdentityResult identityResult, string userId)> CreateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);
            return (result, user.Id);
        }

        public async Task<(IdentityResult identityResult, string userId)> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result, user.Id);
        }

        public async Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return (result, user.Id);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(userId));
            return result;
        }

        public async Task<IdentityResult> CreateUserRoleAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var selectRole = await _roleManager.FindByIdAsync(roleId);

            return await _userManager.AddToRoleAsync(user, selectRole.Name);
        }

        public async Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(string userId, int page = 1, int pageSize = 10)
        {
            var roles = from role in _context.Roles
                        join user in _context.UserRoles on role.Id equals user.RoleId
                        where user.UserId.Equals(userId)
                        select role;

            return await roles.ToListAsync();
        }

        public async Task<IdentityResult> DeleteUserRoleAsync(string userId, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.RemoveFromRoleAsync(user, role.Name);
        }

        public async Task<IEnumerable<ApplicationUserClaim>> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10)
        {
            return await _context.UserClaims.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ApplicationUserClaim> GetUserClaimAsync(string userId, int claimId)
        {
            return await _context.UserClaims.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == claimId) ?? new ApplicationUserClaim();
        }

        public async Task<IdentityResult> CreateUserClaimsAsync(ApplicationUserClaim claims)
        {
            var user = await _userManager.FindByIdAsync(claims.UserId);
            return await _userManager.AddClaimAsync(user, new Claim(claims.ClaimType, claims.ClaimValue));
        }

        public async Task<IdentityResult> UpdateUserClaimsAsync(ApplicationUserClaim claims)
        {
            var user = await _userManager.FindByIdAsync(claims.UserId);
            var userClaim = await _context.UserClaims.Where(x => x.Id == claims.Id).FirstOrDefaultAsync();

            if (userClaim == null)
            {
                throw new NotFoundException("User Claim Information not found. Can not update");
            }
            await _userManager.RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            return await _userManager.AddClaimAsync(user, new Claim(claims.ClaimType, claims.ClaimValue));
        }

        public async Task<IdentityResult> DeleteUserClaimAsync(string userId, int claimId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userClaim = await _context.UserClaims.Where(x => x.Id == claimId).FirstOrDefaultAsync();

            if (userClaim == null)
            {
                throw new NotFoundException("User Claim information not found");
            }
            return await _userManager.RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
        }

        public async Task<List<UserLoginInfo>> GetUserProvidersAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userLoginInfos = await _userManager.GetLoginsAsync(user);

            return userLoginInfos.ToList();
        }

        public async Task<IdentityResult?> DeleteUserProvidersAsync(string userId, string providerKey, string loginProvider)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var login = await _context.UserLogins.Where(x => x.UserId.Equals(userId) && x.ProviderKey == providerKey && x.LoginProvider == loginProvider).FirstOrDefaultAsync();
            if (login != null) return await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
            return null;
        }

        public async Task<ApplicationUserLogin?> GetUserProviderAsync(string userId, string providerKey)
        {
            return await _context.UserLogins.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.ProviderKey == providerKey);
        }

        public async Task<IdentityResult> UserChangePasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> CreateRoleClaimsAsync(ApplicationUserRoleClaim claims)
        {
            var role = await _roleManager.FindByIdAsync(claims.RoleId);
            await _context.RoleClaims.AddAsync(claims);
            return await _roleManager.AddClaimAsync(role, new Claim(claims.ClaimType, claims.ClaimValue));
        }

        public async Task<IdentityResult> UpdateRoleClaimsAsync(ApplicationUserRoleClaim claims)
        {
            var role = await _roleManager.FindByIdAsync(claims.RoleId);
            var userClaim = await _context.UserClaims.Where(x => x.Id == claims.Id).SingleOrDefaultAsync();

            if (userClaim != null)
                await _roleManager.RemoveClaimAsync(role, new Claim(userClaim.ClaimType, userClaim.ClaimValue));

            return await _roleManager.AddClaimAsync(role, new Claim(claims.ClaimType, claims.ClaimValue));
        }

        public async Task<IEnumerable<ApplicationUserRoleClaim>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10)
        {
            var claims = await _context.RoleClaims.Where(x => x.RoleId.Equals(roleId))
                .ToListAsync();
            return claims;
        }

        public async Task<IEnumerable<ApplicationUserRoleClaim>> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10)
        {
            return await _context.UserRoles.Where(x => x.UserId == userId)
                .Join(_context.RoleClaims,
                    ur => ur.RoleId,
                    rc => rc.RoleId,
                    (ur, rc) => rc)
                .ToListAsync();
        }

        public async Task<ApplicationUserRoleClaim?> GetRoleClaimAsync(string roleId, int claimId)
        {
            return await _context.RoleClaims.Where(x => x.RoleId.Equals(roleId) && x.Id == claimId)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityResult?> DeleteRoleClaimAsync(string roleId, int claimId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var roleClaim = await _context.RoleClaims.Where(x => x.Id == claimId).SingleOrDefaultAsync();

            if (roleClaim != null)
                return await _roleManager.RemoveClaimAsync(role, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
            return null;
        }

        public async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }
    }
}
