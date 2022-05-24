using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>()
        {
            new ApiResource("identity_token_service_api", "Identity Token Service API")
            {
                ApiSecrets = new List<Secret>()
                {
                    new Secret("b7db367c-d1aa-0d20-88bd-3ac7ca8ff0d3".Sha256())
                },
                Scopes = new List<string>()
                {
                    "identity_token_service_api"
                },
                UserClaims = new List<string>()
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Id,
                    JwtClaimTypes.Role,
                    "permissions"
                },
                Enabled = true,
                ShowInDiscoveryDocument = true
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
        {
            new ApiScope("identity_token_service_api","identity_token_service_api"),
            new ApiScope("roles", "User Roles")
            {
                UserClaims = {"role"}
            },
            new ApiScope("permissions","Role-Based Authorization Control")
            {
                UserClaims = {"permission"}
            }
        };


        public static IEnumerable<Client> ApiClients => new List<Client>()
        {

            // For swagger endpoint of ISTS.API
            new Client()
            {
                ClientId = "identity_token_service_api_swagger_ui",
                ClientName = "Identity Token Service Swagger",
                AllowedGrantTypes = { GrantType.AuthorizationCode, GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
                ClientSecrets =
                {
                    new Secret("SuperSecret".Sha256())
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "identity_token_service_api",
                    "permissions",
                    "roles"
                },
                AllowOfflineAccess = true,

                // API URL
                RedirectUris = { "https://localhost:7270/swagger/oauth2-redirect.html" },
                AllowedCorsOrigins = { "https://localhost:7270" }, //Allowed Cors originmust be without trailing slash
                RequirePkce = true,
                RequireClientSecret = true,
            },

            // For Frontend application 
            new Client()
            {
                ClientId = "identity_configuration_admin_ui",
                ClientName = "Identity Token Management UI",
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "identity_token_service_api",
                    "roles",
                    "permissions",
                },
                AllowOfflineAccess = true,
                RedirectUris = { "http://localhost:3000/auth-callback" },
                AllowedCorsOrigins = { "http://localhost:3000" },
                PostLogoutRedirectUris = {"http://localhost:3000/"},
                RequirePkce = true,
                RequireClientSecret = false,
                AllowAccessTokensViaBrowser = true
            },
        };




    }
}
