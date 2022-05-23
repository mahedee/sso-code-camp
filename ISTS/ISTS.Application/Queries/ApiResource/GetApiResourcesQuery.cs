using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.ApiResource
{
    public class GetApiResourcesQuery : IRequest<IEnumerable<ApiResourceDto>>
    {
        public string SearchText { get; set; }
    }

    public class GetApiResourcesQueryHandler : IRequestHandler<GetApiResourcesQuery, IEnumerable<ApiResourceDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApiResourceRepository _resourceRepository;

        public GetApiResourcesQueryHandler(IMapper mapper, IApiResourceRepository resourceRepository)
        {
            _mapper = mapper;
            _resourceRepository = resourceRepository;
        }

        public async Task<IEnumerable<ApiResourceDto>> Handle(GetApiResourcesQuery request, CancellationToken cancellationToken)
        {
            return _mapper
                .Map<IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource>, IEnumerable<ApiResourceDto>>(
                    await _resourceRepository.GetApiResourcesAsync(request.SearchText));
        }
    }
}
