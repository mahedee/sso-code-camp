using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.User
{
    public class GetClaimsByUserIdQuery : IRequest<IEnumerable<UserClaimDto>>
    {
        public GetClaimsByUserIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }

    public class GetClaimsByUserIdQueryHandler : IRequestHandler<GetClaimsByUserIdQuery, IEnumerable<UserClaimDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public GetClaimsByUserIdQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<IEnumerable<UserClaimDto>> Handle(GetClaimsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.UserId);
            if (userDetails == null)
            {
                throw new NotFoundException("User Information not found");
            }

            return _mapper.Map<IEnumerable<ApplicationUserClaim>, IEnumerable<UserClaimDto>>(
                await _identityRepository.GetUserClaimsAsync(request.UserId));
        }
    }
}
