using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Commands.ApiScope
{
    public class CreateApiScopeCommand : IRequest<ApiScopeDto>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public List<string>? UserClaims { get; set; }
    }

    public class CreateApiScopeCommandHandler : IRequestHandler<CreateApiScopeCommand, ApiScopeDto>
    {
        private readonly IMapper _mapper;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public CreateApiScopeCommandHandler(IMapper mapper, IApiScopeRepository apiScopeRepository, IIdentityResourceRepository identityResourceRepository)
        {
            _mapper = mapper;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceRepository = identityResourceRepository;
        }

        public async Task<ApiScopeDto> Handle(CreateApiScopeCommand request, CancellationToken cancellationToken)
        {
            var apiScope =
                _mapper.Map<CreateApiScopeCommand, IdentityServer4.EntityFramework.Entities.ApiScope>(request);
            apiScope.Enabled = true;
            apiScope.Required = false;
            apiScope.Emphasize = false;
            apiScope.ShowInDiscoveryDocument = true;

            if (!await _apiScopeRepository.CanInsertApiScopeAsync(apiScope))
            {
                throw new Exception("Api Scope can not added");
            }

            var isExistsIdentityResourceWithSameName =
                await _identityResourceRepository.IsExistIdentityResourceAsync(apiScope.Name);
            if (isExistsIdentityResourceWithSameName)
            {
                throw new Exception("There is an Identity Resource with the Same Name");
            }

            var createdApiScopeId = await _apiScopeRepository.AddApiScopeAsync(apiScope);

            return _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiScope, ApiScopeDto>(
                await _apiScopeRepository.GetApiScopeAsync(createdApiScopeId));
        }
    }
}
