using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.Role
{
    public class DeleteRoleClaimCommand : IRequest
    {
        public string RoleId { get; set; }
        public int ClaimId { get; set; }
    }

    public class DeleteRoleClaimCommandHandler : IRequestHandler<DeleteRoleClaimCommand>
    {
        private readonly IIdentityRepository _identityRepository;

        public DeleteRoleClaimCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<Unit> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityRepository.DeleteRoleClaimAsync(request.RoleId, request.ClaimId);
            if (result == null)
            {
                throw new NotFoundException("Identity result not found");
            }
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Delete Role Claim");
            }
            return Unit.Value;
        }
    }
}
