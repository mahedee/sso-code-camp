using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.IdentityResource
{
    public class CreateIdentityResourceCommand : IRequest<int>
    {
        public CreateIdentityResourceCommand()
        {
            this.UserClaims = new List<string>();
        }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public bool ShowInDiscoveryDocument { get; set; } = true;

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public List<string> UserClaims { get; set; }
    }

    public class CreateIdentityResourceCommandHandler : IRequestHandler<CreateIdentityResourceCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public CreateIdentityResourceCommandHandler(IMapper mapper, IIdentityResourceRepository identityResourceRepository)
        {
            _mapper = mapper;
            _identityResourceRepository = identityResourceRepository;
        }

        public async Task<int> Handle(CreateIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            var identityResource =
                _mapper.Map<CreateIdentityResourceCommand, IdentityServer4.EntityFramework.Entities.IdentityResource>(request);

            if (!await _identityResourceRepository.IsExistsIdentityResourceAsync(identityResource))
            {
                //TODO: Already exists exception need to implement
                throw new Exception("Identity Resource already exists");
            }

            var result = await _identityResourceRepository.AddIdentityResourceAsync(identityResource);
            return result;
        }
    }
}
