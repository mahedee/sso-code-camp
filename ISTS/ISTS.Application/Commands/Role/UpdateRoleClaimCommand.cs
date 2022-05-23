using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.Role
{
    public class UpdateRoleClaimCommand : IRequest
    {

        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class UpdateRoleClaimCommandHandler : IRequestHandler<UpdateRoleClaimCommand>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;
        public UpdateRoleClaimCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<Unit> Handle(UpdateRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var roleClaimDetails = await _identityRepository.GetRoleClaimAsync(request.RoleId, request.Id);
            if (roleClaimDetails == null)
            {
                throw new Exception("Role claims information not found");
            }

            var roleClaim = _mapper.Map<UpdateRoleClaimCommand, ApplicationUserRoleClaim>(request, roleClaimDetails);
            var result = await _identityRepository.UpdateRoleClaimsAsync(roleClaim);
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Delete Role");
            }

            return Unit.Value;
        }
    }
}
