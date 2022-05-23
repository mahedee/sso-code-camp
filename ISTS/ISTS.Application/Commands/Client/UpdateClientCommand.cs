using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Commands.Client
{
    public class UpdateClientCommand : IRequest
    {
        public UpdateClientCommand()
        {
            AllowedScopes = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            RedirectUris = new List<string>();
            //IdentityProviderRestrictions = new List<string>();
            AllowedCorsOrigins = new List<string>();
            AllowedGrantTypes = new List<string>();
            Claims = new List<ClientClaimDto>();
            //Properties = new List<ClientPropertyDto>();
        }
        public int Id { get; set; }
        //public ClientType ClientType { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; } = true;
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        public int AccessTokenLifetime { get; set; } = 3600;
        public int? ConsentLifetime { get; set; }
        public int AccessTokenType { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowRememberConsent { get; set; } = true;
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AuthorizationCodeLifetime { get; set; } = 300;
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
        public int IdentityTokenLifetime { get; set; } = 300;
        public bool IncludeJwtId { get; set; } = true;
        public string LogoUri { get; set; }
        public string ClientClaimsPrefix { get; set; } = "client_";
        public string PairWiseSubjectSalt { get; set; }
        public string ProtocolType { get; set; } = "oidc";
        public int RefreshTokenExpiration { get; set; } = 1;
        public int RefreshTokenUsage { get; set; } = 1;
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        public bool RequireClientSecret { get; set; } = true;
        public bool RequireConsent { get; set; } = true;
        public bool RequirePkce { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        //public List<string> IdentityProviderRestrictions { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> AllowedCorsOrigins { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<ClientClaimDto> Claims { get; set; }
        //public List<ClientPropertyDto> Properties { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public int DeviceCodeLifetime { get; set; } = 300;
        public bool RequireRequestObject { get; set; }
        //public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; }
    }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var clientDetails = await _clientRepository.GetClientAsync(request.Id);
            if (clientDetails == null)
            {
                throw new NotFoundException("Client Information not found");
            }

            var client = _mapper.Map<UpdateClientCommand, IdentityServer4.EntityFramework.Entities.Client>(request, clientDetails);
            await _clientRepository.UpdateClientAsync(client);

            return Unit.Value;
        }
    }
}
