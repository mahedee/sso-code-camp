using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.RolePermission
{
    public class DeleteRolePermissionsByRoleIdCommand : IRequest<int>
    {
        public DeleteRolePermissionsByRoleIdCommand(string roleId)
        {
            RoleId = roleId;
        }

        public string RoleId { get; private set; }
    }

    public class DeleteRolePermissionsByRoleIdCommandHandler : IRequestHandler<DeleteRolePermissionsByRoleIdCommand, int>
    {
        private readonly IRolePermissionsRepository _rolePermissionsRepository;

        public DeleteRolePermissionsByRoleIdCommandHandler(IRolePermissionsRepository rolePermissionsRepository)
        {
            _rolePermissionsRepository = rolePermissionsRepository;
        }

        public async Task<int> Handle(DeleteRolePermissionsByRoleIdCommand request, CancellationToken cancellationToken)
        {
            var rolePermission = (await _rolePermissionsRepository.GetRolePermissions(request.RoleId)).ToList();
            if (rolePermission.Any())
            {
                return await _rolePermissionsRepository.DeleteRolePermissions(rolePermission);
            }

            return 0;
        }
    }
}
