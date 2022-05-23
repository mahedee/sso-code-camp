using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.RolePermission
{
    public class DeleteRolePermissionCommand : IRequest<int>
    {
        public DeleteRolePermissionCommand(string roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }


        public string RoleId { get; private set; }
        public int PermissionId { get; private set; }
    }

    public class DeleteRolePermissionCommandHandler : IRequestHandler<DeleteRolePermissionCommand, int>
    {
        private readonly IRolePermissionsRepository _rolePermissionsRepository;

        public DeleteRolePermissionCommandHandler(IRolePermissionsRepository rolePermissionsRepository)
        {
            _rolePermissionsRepository = rolePermissionsRepository;
        }

        public async Task<int> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rolePermission =
                await _rolePermissionsRepository.GetRolePermissionDetails(request.RoleId, request.PermissionId);
            if (rolePermission == null)
            {
                throw new NotFoundException();
            }
            var result = await _rolePermissionsRepository.DeleteRolePermissions(rolePermission);
            return 1;
        }
    }
}
