using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class DeleteUserClaimCommand : IRequest<int>
    {
        public DeleteUserClaimCommand(string userId, int claimId)
        {
            UserId = userId;
            ClaimId = claimId;
        }

        public string UserId { get; private set; }
        public int ClaimId { get; private set; }
    }

    public class DeleteUserClaimCommandHandler : IRequestHandler<DeleteUserClaimCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public DeleteUserClaimCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(DeleteUserClaimCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityRepository.DeleteUserClaimAsync(request.UserId, request.ClaimId);

            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Delete User Claim");
            }

            return 1;
        }
    }
}
