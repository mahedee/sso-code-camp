using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.User
{
    public class GetRolesByUserIdQuery : IRequest<IEnumerable<RoleDto>>
    {
        public GetRolesByUserIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }

    public class GetRolesByUserIdQueryHandler : IRequestHandler<GetRolesByUserIdQuery, IEnumerable<RoleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public GetRolesByUserIdQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.UserId);
            if (userDetails == null)
                throw new NotFoundException("User Information not found");

            return _mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<RoleDto>>(await _identityRepository.GetUserRolesAsync(request.UserId));
        }
    }
}
