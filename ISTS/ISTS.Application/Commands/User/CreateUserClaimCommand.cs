using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class CreateUserClaimCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class CreateUserClaimCommandHandler : IRequestHandler<CreateUserClaimCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public CreateUserClaimCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(CreateUserClaimCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.UserId);
            if (userDetails == null)
                throw new NotFoundException("User Information not found");

            var claims = _mapper.Map<CreateUserClaimCommand, ApplicationUserClaim>(request);

            var result = await _identityRepository.CreateUserClaimsAsync(claims);

            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Create User Claim");
            }

            return 1;
        }
    }
}
