using ISTS.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IIdentityRepository
    {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);
        Task<bool> IsUserInRoleAsync(string role, string userId);

        Task<IEnumerable<ApplicationUser>> GetUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<IEnumerable<ApplicationUser>> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);

        Task<IEnumerable<ApplicationUser>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        Task<IEnumerable<ApplicationRole>> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, string roleId)> CreateRoleAsync(ApplicationRole role);

        Task<ApplicationRole> GetRoleAsync(string roleId);

        Task<List<ApplicationRole>> GetRolesAsync();

        Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(ApplicationRole role);

        Task<ApplicationUser> GetUserAsync(string userId);

        Task<(IdentityResult identityResult, string userId)> CreateUserAsync(ApplicationUser user);
        Task<(IdentityResult identityResult, string userId)> CreateUserAsync(ApplicationUser user, string password);

        Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(ApplicationUser user);

        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<IdentityResult> CreateUserRoleAsync(string userId, string roleId);

        Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(string userId, int page = 1, int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(string userId, string roleId);

        Task<IEnumerable<ApplicationUserClaim>> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10);

        Task<ApplicationUserClaim> GetUserClaimAsync(string userId, int claimId);

        Task<IdentityResult> CreateUserClaimsAsync(ApplicationUserClaim claims);

        Task<IdentityResult> UpdateUserClaimsAsync(ApplicationUserClaim claims);

        Task<IdentityResult> DeleteUserClaimAsync(string userId, int claimId);

        Task<List<UserLoginInfo>> GetUserProvidersAsync(string userId);

        Task<IdentityResult?> DeleteUserProvidersAsync(string userId, string providerKey, string loginProvider);

        Task<ApplicationUserLogin?> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(string userId, string password);

        Task<IdentityResult> CreateRoleClaimsAsync(ApplicationUserRoleClaim claims);

        Task<IdentityResult> UpdateRoleClaimsAsync(ApplicationUserRoleClaim claims);

        Task<IEnumerable<ApplicationUserRoleClaim>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<IEnumerable<ApplicationUserRoleClaim>> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

        Task<ApplicationUserRoleClaim?> GetRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult?> DeleteRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);
    }
}
