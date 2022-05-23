using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.Role
{
    public class GetRolesQuery : IRequest<IEnumerable<RoleDto>>
    {
        public string? SearchText { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public GetRolesQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _identityRepository.GetRolesAsync(request.SearchText, request.PageNumber, request.PageSize);
            return _mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<RoleDto>>(roles);
        }
    }
}
