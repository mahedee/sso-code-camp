using IdentityServer4.Models;
using IdentityServer4.Services;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRolePermissionsRepository _rolePermissionsRepository;
        private readonly IApiResourceRepository _apiResourceRepository;

        public ProfileService(UserManager<ApplicationUser> userManager, IRolePermissionsRepository rolePermissionsRepository, RoleManager<ApplicationRole> roleManager, IApiResourceRepository apiResourceRepository)
        {
            _userManager = userManager;
            _rolePermissionsRepository = rolePermissionsRepository;
            _roleManager = roleManager;
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>();

            var userClaims = await _userManager.GetClaimsAsync(user);
            if (userClaims.Any())
            {
                claims.AddRange(userClaims);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
            {
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim("role", userRole));

                    var clientInfo = context.Client;
                    foreach (var allowedScope in clientInfo.AllowedScopes)
                    {
                        var apiResource = await _apiResourceRepository.GetApiResourceByNameAsync(allowedScope);
                        if (apiResource == null) continue;
                        var roleDetails = await _roleManager.FindByNameAsync(userRole);

                        var rolePermissions = (await _rolePermissionsRepository.GetRolePermissions(roleDetails.Id, apiResource.Id)).ToList();
                        if (rolePermissions.Any())
                        {
                            claims.AddRange(rolePermissions.Select(rolePermission => new Claim("permissions", rolePermission.Permission.Name)));
                        }
                    }
                }
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null);
        }
    }
}
