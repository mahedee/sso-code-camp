using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.Permission
{
    public class DeletePermissionCommand : IRequest<int>
    {
        public DeletePermissionCommand(int permissionId)
        {
            PermissionId = permissionId;
        }

        public int PermissionId { get; private set; }
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, int>
    {
        private readonly IPermissionRepository _permissionRepository;


        public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<int> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            await _permissionRepository.DeletePermission(request.PermissionId);
            return 1;
        }
    }
}
