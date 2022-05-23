using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.Role
{
    public class UpdateRoleCommand : IRequest<RoleDto>
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string? RoleDetails { get; set; }
    }
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public UpdateRoleCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRoleDetails = await _identityRepository.GetRoleAsync(request.Id);
            if (existingRoleDetails == null)
            {
                throw new NotFoundException("Role Information not found");
            }
            var role = _mapper.Map<UpdateRoleCommand, ApplicationRole>(request, existingRoleDetails);
            var (identityResult, roleId) = await _identityRepository.UpdateRoleAsync(role);
            if (!identityResult.Succeeded)
            {
                throw new IdentityResultException(identityResult, "Update Role");
            }

            var roleDetails = await _identityRepository.GetRoleAsync(roleId);
            return _mapper.Map<ApplicationRole, RoleDto>(roleDetails);
        }
    }
}
