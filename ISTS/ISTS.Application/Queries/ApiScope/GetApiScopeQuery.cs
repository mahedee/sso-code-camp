using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.ApiScope
{
    public class GetApiScopeQuery : IRequest<IEnumerable<ApiScopeDto>>
    {
        public string SearchText { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetApiScopeQueryHandler : IRequestHandler<GetApiScopeQuery, IEnumerable<ApiScopeDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApiScopeRepository _apiScopeRepository;

        public GetApiScopeQueryHandler(IMapper mapper, IApiScopeRepository apiScopeRepository)
        {
            _mapper = mapper;
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<IEnumerable<ApiScopeDto>> Handle(GetApiScopeQuery request, CancellationToken cancellationToken)
        {
            return _mapper
                .Map<IEnumerable<IdentityServer4.EntityFramework.Entities.ApiScope>, IEnumerable<ApiScopeDto>>(
                    await _apiScopeRepository.GetApiScopesAsync(request.SearchText, request.PageNo, request.PageSize));
        }
    }
}
