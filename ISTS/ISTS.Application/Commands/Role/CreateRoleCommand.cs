using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.Role
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public string RoleName { get; set; }
        public string? RoleDetails { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;
        //private readonly IAggregateRepository<ApplicationRoleAggregate> _aggregateRepository;
        public CreateRoleCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<CreateRoleCommand, ApplicationRole>(request);
            var (result, roleId) = await _identityRepository.CreateRoleAsync(role);
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Create Role");
            }
            var createdRole = await _identityRepository.GetRoleAsync(roleId);

            //Event Sourcing Audit Log
            //var roleEvents = await _aggregateRepository.LoadAsync(roleId);
            //roleEvents.CreateRole(createdRole);

            //await _aggregateRepository.SaveAsync(roleEvents);

            return _mapper.Map<ApplicationRole, RoleDto>(createdRole);
        }
    }
}
