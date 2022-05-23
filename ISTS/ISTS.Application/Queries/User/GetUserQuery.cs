using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.User
{
    public class GetUserQuery : IRequest<IEnumerable<UserDto>>
    {
        public string SearchText { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 1000;
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IEnumerable<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;
        public GetUserQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDto>>(
                await _identityRepository.GetUsersAsync(request.SearchText));
        }
    }
}
