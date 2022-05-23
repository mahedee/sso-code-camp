using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Common.Models;
using ISTS.Application.Dtos;
using ISTS.Core.Enums;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using MediatR;

namespace ISTS.Application.Commands.Client
{
    public class CreateClientCommand : IRequest<ClientDto>
    {
        public CreateClientCommand()
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
        public ClientType ClientType { get; set; }
        //public string ClientId { get; set; }
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

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDto>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public CreateClientCommandHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            PrepareClientTypeForNewClient(request);
            var client = _mapper.Map<CreateClientCommand, IdentityServer4.EntityFramework.Entities.Client>(request);

            client.ClientId = await PrepareClientId();
            var clientSecret = Guid.NewGuid().ToString("N");

            client.ClientSecrets = new List<ClientSecret>()
            {
                new ClientSecret()
                {
                    Value = clientSecret.Sha256()
                }
            };

            var clientId = await _clientRepository.AddClientAsync(client);
            var clientDetails = new ClientDetails()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var result = await _clientRepository.AddClientDetailsAsync(clientDetails);


            var addedClient = await _clientRepository.GetClientAsync(clientId);
            if (addedClient == null)
            {
                throw new NotFoundException("Client Information not found");
            }
            var mappedClient = _mapper.Map<IdentityServer4.EntityFramework.Entities.Client, ClientDto>(addedClient);
            mappedClient.ClientSecret = clientSecret;

            return mappedClient;
        }

        private void PrepareClientTypeForNewClient(CreateClientCommand client)
        {
            switch (client.ClientType)
            {
                case ClientType.Empty:
                    break;
                case ClientType.Web:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = true;
                    break;
                case ClientType.Spa:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Native:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Machine:
                    client.AllowedGrantTypes.AddRange(GrantTypes.ClientCredentials);
                    break;
                case ClientType.Device:
                    client.AllowedGrantTypes.AddRange(GrantTypes.DeviceFlow);
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!client.AllowedScopes.Any())
            {
                client.AllowedScopes = new List<string>()
                {
                    "openid", "profile"
                };
            }
        }

        private async Task<string> PrepareClientId()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var clientId = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var clientDetails = await _clientRepository.GetClientAsync(clientId);

            if (clientDetails != null)
            {
                await PrepareClientId();
            }

            return clientId;
        }
    }
}
