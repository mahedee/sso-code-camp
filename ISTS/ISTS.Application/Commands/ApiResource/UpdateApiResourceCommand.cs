using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Commands.ApiResource
{
    public class UpdateApiResourceCommand : IRequest<ApiResourceDto>
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = false;
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
        public bool NonEditable { get; set; }
    }

    public class UpdateApiResourceCommandHandler : IRequestHandler<UpdateApiResourceCommand, ApiResourceDto>
    {
        private readonly IMapper _mapper;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;

        public UpdateApiResourceCommandHandler(IMapper mapper, IApiResourceRepository apiResourceRepository, IApiScopeRepository apiScopeRepository)
        {
            _mapper = mapper;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<ApiResourceDto> Handle(UpdateApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResourceInfo = await _apiResourceRepository.GetApiResourceAsync(request.Id);
            if (apiResourceInfo == null)
            {
                throw new NotFoundException("Api Resource information not found");
            }
            var apiResource =
                _mapper.Map<UpdateApiResourceCommand, IdentityServer4.EntityFramework.Entities.ApiResource>(request, apiResourceInfo);

            var canInsert = await _apiResourceRepository.CanInsertApiResourceAsync(apiResource);
            if (!canInsert)
            {
                throw new Exception("Api Resource already exists");
            }

            var updatedApiResourceId = await _apiResourceRepository.UpdateApiResourceAsync(apiResource);


            var apiResourceDetails = await _apiResourceRepository.GetApiResourceAsync(updatedApiResourceId);

            //Update Scopes Table
            //Update API Scope
            //Update ApiResourceScope Table
            //Update Client Scope Table

            return _mapper
                .Map<IdentityServer4.EntityFramework.Entities.ApiResource, ApiResourceDto>(apiResourceDetails);
        }
    }
}
