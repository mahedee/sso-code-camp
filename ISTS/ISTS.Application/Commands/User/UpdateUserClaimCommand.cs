using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class UpdateUserClaimCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public string ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class UpdateUserClaimCommandHandler : IRequestHandler<UpdateUserClaimCommand, int>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;

        public UpdateUserClaimCommandHandler(IIdentityRepository identityRepository, IMapper mapper)
        {
            _identityRepository = identityRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateUserClaimCommand request, CancellationToken cancellationToken)
        {
            var userClaim = _mapper.Map<UpdateUserClaimCommand, ApplicationUserClaim>(request);

            var updateUserClaimResult = await _identityRepository.UpdateUserClaimsAsync(userClaim);
            if (!updateUserClaimResult.Succeeded)
            {
                throw new IdentityResultException(updateUserClaimResult, "Update User Claim");
            }

            return 1;
        }
    }
}
