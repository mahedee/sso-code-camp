using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.Role
{
    public class GetUsersByRoleIdQuery : IRequest<IEnumerable<UserDto>>
    {
        public string RoleId { get; set; }
    }

    public class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, IEnumerable<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public GetUsersByRoleIdQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDto>>(
                await _identityRepository.GetRoleUsersAsync(request.RoleId, ""));
        }
    }
}
