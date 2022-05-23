using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using IdentityServer4.EntityFramework.Entities;
using MediatR;

namespace ISTS.Application.Queries.ApiResource
{
    public class GetSecretsByApiResourceIdQuery : IRequest<IEnumerable<ApiResourceSecretDto>>
    {
        public GetSecretsByApiResourceIdQuery(int apiResourceId)
        {
            ApiResourceId = apiResourceId;
        }

        public int ApiResourceId { get; private set; }
    }

    public class GetSecretsByApiResourceIdQueryHandler : IRequestHandler<GetSecretsByApiResourceIdQuery, IEnumerable<ApiResourceSecretDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApiResourceRepository _resourceRepository;

        public GetSecretsByApiResourceIdQueryHandler(IMapper mapper, IApiResourceRepository resourceRepository)
        {
            _mapper = mapper;
            _resourceRepository = resourceRepository;
        }

        public async Task<IEnumerable<ApiResourceSecretDto>> Handle(GetSecretsByApiResourceIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ApiResourceSecret>, IEnumerable<ApiResourceSecretDto>>(
                await _resourceRepository.GetApiSecretsAsync(request.ApiResourceId));
        }
    }
}
