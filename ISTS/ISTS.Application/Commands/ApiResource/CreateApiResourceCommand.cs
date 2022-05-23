using AutoMapper;
using IdentityModel;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using MediatR;

namespace ISTS.Application.Commands.ApiResource
{
    public class CreateApiResourceCommand : IRequest<ApiResourceDto>
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = false;

        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; } = new List<string>()
        {
            JwtClaimTypes.Email,
            JwtClaimTypes.Name,
            JwtClaimTypes.Id,
            JwtClaimTypes.Role
        };
        public bool NonEditable { get; set; }
    }

    public class CreateApiResourceCommandHandler : IRequestHandler<CreateApiResourceCommand, ApiResourceDto>
    {
        private readonly IMapper _mapper;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        public CreateApiResourceCommandHandler(IMapper mapper, IApiResourceRepository apiResourceRepository, IApiScopeRepository apiScopeRepository)
        {
            _mapper = mapper;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<ApiResourceDto> Handle(CreateApiResourceCommand request, CancellationToken cancellationToken)
        {
            var mappedApiResource =
                _mapper.Map<CreateApiResourceCommand, IdentityServer4.EntityFramework.Entities.ApiResource>(request);

            var canInsert = await _apiResourceRepository.CanInsertApiResourceAsync(mappedApiResource);
            if (!canInsert)
            {
                throw new Exception("Api Resource already exists");
            }

            var apiScopeInfo = new IdentityServer4.EntityFramework.Entities.ApiScope()
            {
                Name = mappedApiResource.Name,
                DisplayName = mappedApiResource.DisplayName,
                Description = mappedApiResource.Description,
                Enabled = true,
                ShowInDiscoveryDocument = false,
                Required = false,
                Emphasize = false
            };

            var canInsertApiScope = await _apiScopeRepository.CanInsertApiScopeAsync(apiScopeInfo);
            if (canInsertApiScope)
            {
                var result = await _apiScopeRepository.AddApiScopeAsync(apiScopeInfo);
                if (result > 0)
                {
                    mappedApiResource.Scopes.Add(new ApiResourceScope()
                    {
                        Scope = apiScopeInfo.Name
                    });
                }
            }
            else
            {
                mappedApiResource.Scopes.Add(new ApiResourceScope()
                {
                    Scope = apiScopeInfo.Name
                });
            }

            var createdApiResourceId = await _apiResourceRepository.AddApiResourceAsync(mappedApiResource);

            var apiResource = await _apiResourceRepository.GetApiResourceAsync(createdApiResourceId);
            return _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiResource, ApiResourceDto>(apiResource);
        }
    }
}
