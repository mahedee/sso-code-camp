using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Commands.ApiScope
{
    public class UpdateApiScopeCommand : IRequest<ApiScopeDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool Enabled { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<string> UserClaims { get; set; }
    }

    public class UpdateApiScopeCommandHandler : IRequestHandler<UpdateApiScopeCommand, ApiScopeDto>
    {
        private readonly IMapper _mapper;
        private readonly IApiScopeRepository _apiScopeRepository;

        public UpdateApiScopeCommandHandler(IMapper mapper, IApiScopeRepository apiScopeRepository)
        {
            _mapper = mapper;
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<ApiScopeDto> Handle(UpdateApiScopeCommand request, CancellationToken cancellationToken)
        {
            var apiScopeDetails = await _apiScopeRepository.GetApiScopeAsync(request.Id);
            if (apiScopeDetails == null)
                throw new NotFoundException("Api Scope Details not found");

            var updateApiScope =
                _mapper.Map<UpdateApiScopeCommand, IdentityServer4.EntityFramework.Entities.ApiScope>(request, apiScopeDetails);

            var updatedApiScopeId = await _apiScopeRepository.UpdateApiScopeAsync(updateApiScope);

            return _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiScope, ApiScopeDto>(updateApiScope);
        }
    }
}
