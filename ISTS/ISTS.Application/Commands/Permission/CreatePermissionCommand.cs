using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISTS.Application.Common.Interfaces;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.Permission
{
    public class CreatePermissionCommand : IRequest<int>
    {
        public int ApiResourceId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreatePermissionCommandHandler(IMapper mapper, IPermissionRepository permissionRepository, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permissionData = _mapper.Map<CreatePermissionCommand, Core.Entities.Permission>(request);
            var isPermissionAlreadyExists = await _permissionRepository.IsPermissionExists(permissionData);
            if (isPermissionAlreadyExists)
            {
                throw new Exception("Permission is already exists");
            }
            permissionData.CreatedDate = DateTime.Now;
            permissionData.CreatedBy = _currentUserService.UserId;
            await _permissionRepository.CreatePermission(permissionData);
            return 1;
        }
    }
}
