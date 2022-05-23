using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities;
using MediatR;

namespace ISTS.Application.Commands.RolePermission
{
    public class CreateRolePermissionCommand : IRequest<int>
    {
        public string RoleId { get; set; }
        public List<string> Permissions { get; set; }
        public int ApiResourceId { get; set; }
    }

    public class CreateRolePermissionCommandHandler : IRequestHandler<CreateRolePermissionCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IRolePermissionsRepository _rolePermissionsRepository;
        private readonly IPermissionRepository _permissionRepository;

        public CreateRolePermissionCommandHandler(IMapper mapper, IRolePermissionsRepository rolePermissionsRepository, IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _rolePermissionsRepository = rolePermissionsRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<int> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var counter = 0;
            var listRolePermission = new List<CreateRolePermissionCommandDto>();
            //If there is any permission selected from front-end then below condition will be true
            if (request.Permissions.Any())
            {
                foreach (var requestPermission in request.Permissions)
                {
                    var permissionDetails =
                        await _permissionRepository.GetPermissionDetails(requestPermission, request.ApiResourceId);
                    if (permissionDetails == null)
                    {
                        throw new NotFoundException();
                    }

                    var permissionCommandDto =
                        new CreateRolePermissionCommandDto(request.RoleId, permissionDetails.Id, request.ApiResourceId);

                    listRolePermission.Add(permissionCommandDto);
                }
            }
            else
            {
                //If any permission is not selected in front-end but clicked on the Save button which means User want to remove all permission from the specified role.
                //All permission which are assigned to the role and api will be deleted for below condition
                var permissions =
                    (await _rolePermissionsRepository.GetRolePermissions(request.RoleId, request.ApiResourceId)).ToList();
                if (permissions.Any())
                {
                    await _rolePermissionsRepository.DeleteRolePermissions(permissions);
                }
            }

            if (listRolePermission.Any())
            {
                var rolePermission = _mapper.Map<IEnumerable<CreateRolePermissionCommandDto>, IEnumerable<RolePermissions>>(listRolePermission);

                counter = await _rolePermissionsRepository.CreateRolePermissions(rolePermission, request.RoleId, request.ApiResourceId);
            }

            return counter;
        }
    }
}
