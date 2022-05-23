using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.IdentityResource
{
    public class GetIdentityResourcesQuery : IRequest<IEnumerable<IdentityResourceDto>>
    {
        public string SearchText { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetIdentityResourcesQueryHandler : IRequestHandler<GetIdentityResourcesQuery, IEnumerable<IdentityResourceDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public GetIdentityResourcesQueryHandler(IMapper mapper, IIdentityResourceRepository identityResourceRepository)
        {
            _mapper = mapper;
            _identityResourceRepository = identityResourceRepository;
        }

        public async Task<IEnumerable<IdentityResourceDto>> Handle(GetIdentityResourcesQuery request, CancellationToken cancellationToken)
        {
            return _mapper
                .Map<IEnumerable<IdentityServer4.EntityFramework.Entities.IdentityResource>,
                    IEnumerable<IdentityResourceDto>>(await _identityResourceRepository.GetIdentityResourcesAsync(""));
        }
    }
}
