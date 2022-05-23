using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.Role
{
    public class CreateRoleClaimCommand : IRequest
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class CreateRoleClaimCommandHandler : IRequestHandler<CreateRoleClaimCommand>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public CreateRoleClaimCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<Unit> Handle(CreateRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityRepository.CreateRoleClaimsAsync(_mapper.Map<CreateRoleClaimCommand, ApplicationUserRoleClaim>(request));
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Role Claim");
            }


            return Unit.Value;
        }
    }
}
