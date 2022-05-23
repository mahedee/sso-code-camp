using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ISTS.Application.Commands.RolePermission
{
    public class CreateRolePermissionCommandValidator : AbstractValidator<CreateRolePermissionCommand>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionsRepository _rolePermissionsRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        public CreateRolePermissionCommandValidator(RoleManager<ApplicationRole> roleManager, IPermissionRepository permissionRepository, IApiResourceRepository apiResourceRepository, IRolePermissionsRepository rolePermissionsRepository)
        {
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
            _apiResourceRepository = apiResourceRepository;
            _rolePermissionsRepository = rolePermissionsRepository;
            RuleFor(x => x.RoleId).NotEmpty()
                .WithMessage("Role ID is required").NotNull().WithMessage("Role ID is required")
                .MustAsync(async (roleId, token) =>
                {
                    var roleInfo = await _roleManager.FindByIdAsync(roleId);
                    return roleInfo != null;
                }).WithMessage("Invalid role information");

            RuleForEach(x => x.Permissions).NotNull().WithMessage("Permission ID is required")
                .MustAsync(async (command, permissionName, token) =>
                {
                    var permission =
                        await _permissionRepository.GetPermissionDetails(permissionName, command.ApiResourceId);
                    return permission != null;
                }).WithMessage("Permission not found");
            //.MustAsync(async (command, permissionName, token) =>
            //{
            //    var rolePermissions =
            //        await _rolePermissionsRepository.GetRolePermissions(command.RoleId, command.ApiResourceId);

            //    var permission = await _permissionRepository.GetPermissionDetails(permissionName, command.ApiResourceId);

            //    return !rolePermissions.Any(x =>
            //        permission != null && x.RoleId == command.RoleId && x.PermissionId == permission.Id && x.ApiResourceId == command.ApiResourceId);

            //}).WithMessage("Role Permission already exists");

            RuleFor(x => x.ApiResourceId)
                .NotNull().WithMessage("API information is required").GreaterThan(0)
                .WithMessage("API information is required")
                .MustAsync(async (apiResourceId, token) =>
                {
                    var apiResourceInfo = await _apiResourceRepository.GetApiResourceAsync(apiResourceId);
                    return apiResourceInfo != null;
                }).WithMessage("Invalid API information");
        }
    }
}
