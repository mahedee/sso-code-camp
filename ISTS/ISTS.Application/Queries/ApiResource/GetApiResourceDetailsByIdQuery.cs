using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.ApiResource
{
    public class GetApiResourceDetailsByIdQuery : IRequest<ApiResourceDto>
    {
        public GetApiResourceDetailsByIdQuery(int apiResourceId)
        {
            ApiResourceId = apiResourceId;
        }

        public int ApiResourceId { get; private set; }
    }

    public class GetApiResourceDetailsByIdQueryHandler : IRequestHandler<GetApiResourceDetailsByIdQuery, ApiResourceDto>
    {
        private readonly IMapper _mapper;
        private readonly IApiResourceRepository _resourceRepository;

        public GetApiResourceDetailsByIdQueryHandler(IMapper mapper, IApiResourceRepository resourceRepository)
        {
            _mapper = mapper;
            _resourceRepository = resourceRepository;
        }

        public async Task<ApiResourceDto> Handle(GetApiResourceDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiResource, ApiResourceDto>(
                await _resourceRepository.GetApiResourceAsync(request.ApiResourceId));
        }
    }
}
