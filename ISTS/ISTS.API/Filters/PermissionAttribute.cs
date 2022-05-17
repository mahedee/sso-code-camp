using IdentityServer4.Extensions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ISTS.API.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string Permission { get; set; } = null;
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (string.IsNullOrEmpty(Permission))
            {
                context.Result = new ForbidResult();
                return;
            }

            if (context.HttpContext.User.Identity is { IsAuthenticated: true })
            {
                var userId = context.HttpContext.User.Identity.GetSubjectId();
                if (string.IsNullOrEmpty(userId))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var userManager = (IdentityRepository)context.HttpContext.RequestServices.GetService(typeof(IIdentityRepository));
                if (userManager == null)
                {
                    throw new Exception("Dependency can not be loaded");
                }

                var userRoles = (await userManager.GetUserRolesAsync(userId, 1, Int32.MaxValue)).ToList();
                if (!userRoles.Any())
                {
                    context.Result = new ForbidResult();
                    return;
                }

                foreach (var role in userRoles)
                {
                    var rolePermissionManager =
                        (RolePermissionsRepository)context.HttpContext.RequestServices.GetService(
                            typeof(IRolePermissionsRepository));

                    if (rolePermissionManager == null)
                    {
                        throw new Exception("Dependency can not be loaded");
                    }

                    var rolePermissions = await rolePermissionManager.GetRolePermissions(role.Id);

                    if (rolePermissions.Any(p => p.Permission.Name == Permission))
                    {
                        return;
                    }
                }
            }
            else
            {
                context.Result = new ForbidResult();
                return;
            }

            context.Result = new ForbidResult();
            return;
        }
    }
}
