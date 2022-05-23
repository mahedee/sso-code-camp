using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.ApiScope
{
    public class GetApiScopeByIdQuery : IRequest<ApiScopeDto>
    {
        public GetApiScopeByIdQuery(int apiScopeId)
        {
            ApiScopeId = apiScopeId;
        }

        public int ApiScopeId { get; private set; }
    }

    public class GetApiScopeByIdQueryHandler : IRequestHandler<GetApiScopeByIdQuery, ApiScopeDto>
    {
        private readonly IMapper _mapper;
        private readonly IApiScopeRepository _apiScopeRepository;

        public GetApiScopeByIdQueryHandler(IMapper mapper, IApiScopeRepository apiScopeRepository)
        {
            _mapper = mapper;
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<ApiScopeDto> Handle(GetApiScopeByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiScope, ApiScopeDto>(
                await _apiScopeRepository.GetApiScopeAsync(request.ApiScopeId));
        }
    }
}
