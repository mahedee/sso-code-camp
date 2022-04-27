using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using ISTS.Application.Common.Constants;
using ISTS.Application.Common.Helpers;
using ISTS.Application.Common.Interfaces.DbContexts;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Common.Models;
using ISTS.Core.Enums;
using Microsoft.EntityFrameworkCore;

using Client = IdentityServer4.EntityFramework.Entities.Client;
using ClientClaim = IdentityServer4.EntityFramework.Entities.ClientClaim;


namespace ISTS.Infrastructure.Identity
{
    public class ClientRepository : IClientRepository
    {
        private readonly IAdminConfigurationDbContext _adminConfigurationDbContext;

        public ClientRepository(IAdminConfigurationDbContext adminConfigurationDbContext)
        {
            _adminConfigurationDbContext = adminConfigurationDbContext;
        }

        public async Task<int> AddClientAsync(Client client)
        {
            await _adminConfigurationDbContext.Clients.AddAsync(client);
            await _adminConfigurationDbContext.SaveChangesAsync();
            return client.Id;
        }

        public async Task<int> AddClientDetailsAsync(ClientDetails? clientDetails)
        {
            await _adminConfigurationDbContext.ClientDetails.AddAsync(clientDetails);
            await _adminConfigurationDbContext.SaveChangesAsync();
            return clientDetails.Id;
        }

        public async Task<int> UpdateClientAsync(Client client)
        {
            //Remove old relations
            await RemoveClientRelationsAsync(client);

            //Update with new data
            _adminConfigurationDbContext.Clients.Update(client);
            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public async Task<ClientDetails?> GetClientAdditionalDetailsAsync(int clientId)
        {
            return await _adminConfigurationDbContext.ClientDetails.FirstOrDefaultAsync(x => x.ClientId == clientId);
        }

        public async Task<int> RemoveClientAsync(Client client)
        {
            _adminConfigurationDbContext.Clients.Remove(client);
            var clientAdditionalDetails = await GetClientAdditionalDetailsAsync(client.Id);
            if (clientAdditionalDetails != null)
            {
                _adminConfigurationDbContext.ClientDetails.Remove(clientAdditionalDetails);
            }

            return await _adminConfigurationDbContext.SaveChangesAsync();
        }


        public async Task<Client?> GetClientAsync(int clientId)
        {
            var result = await _adminConfigurationDbContext.Clients.Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .Where(x => x.Id == clientId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Client?> GetClientAsync(string clientId)
        {
            var result = await _adminConfigurationDbContext.Clients.Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .Where(x => x.ClientId == clientId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> IsClientAlreadyExists(Client client)
        {
            if (client.Id == 0)
            {
                var existsWithClientName = await _adminConfigurationDbContext.Clients.Where(x => x.ClientId == client.ClientId).FirstOrDefaultAsync();
                return existsWithClientName == null;
            }
            else
            {
                var existsWithClientName = await _adminConfigurationDbContext.Clients.Where(x => x.ClientId == client.ClientId && x.Id != client.Id).FirstOrDefaultAsync();
                return existsWithClientName == null;
            }
        }

        public async Task<(string? ClientId, string? ClientName)> GetClientIdAsync(int clientId)
        {
            var client = await _adminConfigurationDbContext.Clients.Where(x => x.Id == clientId)
                .Select(x => new { x.ClientId, x.ClientName })
                .SingleOrDefaultAsync();

            return (client?.ClientId, client?.ClientName);
        }

        public async Task<IEnumerable<Client>> GetClientsAsync(string search = "", int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await _adminConfigurationDbContext.Clients.Where(x => x.ClientName.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            return await _adminConfigurationDbContext.Clients.ToListAsync();
        }

        public async Task<List<string>> GetScopesAsync(string scope, int limit = 0)
        {
            var identityResources = await _adminConfigurationDbContext.IdentityResources
                .Where(x => x.Name.Contains(scope))
                .Select(x => x.Name).ToListAsync();

            var apiScopes = await _adminConfigurationDbContext.ApiScopes
                .Where(x => x.Name.Contains(scope))
                .Select(x => x.Name).ToListAsync();

            return identityResources.Concat(apiScopes).ToList();
        }

        public List<string> GetGrantTypes(string grant, int limit = 0)
        {
            return ClientConstants.GetGrantTypes().Where(x => x.Contains(grant)).ToList();
        }

        public List<SelectItem> GetProtocolTypes()
        {
            return ClientConstants.GetProtocolTypes();
        }

        public List<SelectItem> GetAccessTokenTypes()
        {
            var accessTokenTypes = EnumHelpers.ToSelectList<AccessTokenType>();
            return accessTokenTypes;
        }

        public List<SelectItem> GetTokenExpirations()
        {
            var tokenExpirations = EnumHelpers.ToSelectList<TokenExpiration>();
            return tokenExpirations;
        }

        public List<SelectItem> GetTokenUsage()
        {
            var tokenUsage = EnumHelpers.ToSelectList<TokenUsage>();
            return tokenUsage;
        }

        public List<SelectItem> GetHashTypes()
        {
            var tokenUsage = EnumHelpers.ToSelectList<HashType>();
            return tokenUsage;
        }

        public List<SelectItem> GetSecretTypes()
        {
            var secrets = new List<SelectItem>();
            secrets.AddRange(ClientConstants.GetSecretTypes().Select(x => new SelectItem(x, x)));

            return secrets;
        }

        public List<string> GetStandardClaims(string claim, int limit = 0)
        {
            var filteredClaims = ClientConstants.GetStandardClaims()
                .Where(x => x.Contains(claim))
                .ToList();

            return filteredClaims;
        }

        public async Task<int> AddClientSecretAsync(int clientId, ClientSecret clientSecret)
        {
            var client = await _adminConfigurationDbContext.Clients.Where(x => x.Id == clientId).FirstOrDefaultAsync();
            clientSecret.Client = client;

            await _adminConfigurationDbContext.ClientSecrets.AddAsync(clientSecret);
            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteClientSecretAsync(ClientSecret clientSecret)
        {
            var secretToDelete = await _adminConfigurationDbContext.ClientSecrets.Where(x => x.Id == clientSecret.Id).FirstOrDefaultAsync();

            if (secretToDelete == null) return 0;
            _adminConfigurationDbContext.ClientSecrets.Remove(secretToDelete);
            return await _adminConfigurationDbContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<ClientSecret>> GetClientSecretsAsync(int clientId, int page = 1, int pageSize = 10)
        {
            var secrets = await _adminConfigurationDbContext.ClientSecrets
                .Where(x => x.Client.Id == clientId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return secrets;
        }

        public async Task<ClientSecret> GetClientSecretAsync(int clientId, int clientSecretId)
        {
            var clientSecret = await _adminConfigurationDbContext.ClientSecrets
                .Include(x => x.Client)
                .Where(x => x.Id == clientSecretId && x.ClientId == clientId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return clientSecret;
        }

        public async Task<IEnumerable<ClientClaim>> GetClientClaimsAsync(int clientId, int page = 1, int pageSize = 10)
        {
            return await _adminConfigurationDbContext.ClientClaims
                .Include(x => x.Client)
                .Where(x => x.ClientId == clientId).ToListAsync();
        }

        public async Task<IEnumerable<ClientProperty>> GetClientPropertiesAsync(int clientId, int page = 1, int pageSize = 10)
        {
            var properties = await _adminConfigurationDbContext.ClientProperties.Where(x => x.Client.Id == clientId)
                .ToListAsync();
            return properties;
        }

        public async Task<ClientClaim> GetClientClaimAsync(int clientClaimId)
        {
            return await _adminConfigurationDbContext.ClientClaims
                .Include(x => x.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == clientClaimId);
        }

        public async Task<ClientProperty> GetClientPropertyAsync(int clientPropertyId)
        {
            return await _adminConfigurationDbContext.ClientProperties
                .Include(x => x.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == clientPropertyId);
        }

        public async Task<int> AddClientClaimAsync(int clientId, ClientClaim clientClaim)
        {
            var client = await _adminConfigurationDbContext.Clients.Where(x => x.Id == clientId).FirstOrDefaultAsync();

            clientClaim.Client = client;
            await _adminConfigurationDbContext.ClientClaims.AddAsync(clientClaim);
            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public async Task<int> AddClientPropertyAsync(int clientId, ClientProperty clientProperties)
        {
            var client = await _adminConfigurationDbContext.Clients.Where(x => x.Id == clientId).FirstOrDefaultAsync();

            clientProperties.Client = client;
            await _adminConfigurationDbContext.ClientProperties.AddAsync(clientProperties);
            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteClientClaimAsync(ClientClaim clientClaim)
        {
            _adminConfigurationDbContext.ClientClaims.Remove(clientClaim);
            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteClientPropertyAsync(ClientProperty clientProperty)
        {
            _adminConfigurationDbContext.ClientProperties.Remove(clientProperty);
            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public List<string> GetSigningAlgorithms(string algorithm, int limit = 0)
        {
            var signingAlgorithms = ClientConstants.SigningAlgorithms()
                .Where(x => x.Contains(algorithm))
                .OrderBy(x => x)
                .ToList();

            return signingAlgorithms;
        }

        private async Task RemoveClientRelationsAsync(Client client)
        {
            //Remove old allowed scopes
            var clientScopes = await _adminConfigurationDbContext.ClientScopes.Where(x => x.Client.Id == client.Id).ToListAsync();
            _adminConfigurationDbContext.ClientScopes.RemoveRange(clientScopes);

            //Remove old grant types
            var clientGrantTypes = await _adminConfigurationDbContext.ClientGrantTypes.Where(x => x.Client.Id == client.Id).ToListAsync();
            _adminConfigurationDbContext.ClientGrantTypes.RemoveRange(clientGrantTypes);

            //Remove old redirect uri
            var clientRedirectUris = await _adminConfigurationDbContext.ClientRedirectUris.Where(x => x.Client.Id == client.Id).ToListAsync();
            _adminConfigurationDbContext.ClientRedirectUris.RemoveRange(clientRedirectUris);

            //Remove old client cors
            var clientCorsOrigins = await _adminConfigurationDbContext.ClientCorsOrigins.Where(x => x.Client.Id == client.Id).ToListAsync();
            _adminConfigurationDbContext.ClientCorsOrigins.RemoveRange(clientCorsOrigins);

            //Remove old client id restrictions
            var clientIdPRestrictions = await _adminConfigurationDbContext.ClientIdPRestrictions.Where(x => x.Client.Id == client.Id).ToListAsync();
            _adminConfigurationDbContext.ClientIdPRestrictions.RemoveRange(clientIdPRestrictions);

            //Remove old client post logout redirect
            var clientPostLogoutRedirectUris = await _adminConfigurationDbContext.ClientPostLogoutRedirectUris.Where(x => x.Client.Id == client.Id).ToListAsync();
            _adminConfigurationDbContext.ClientPostLogoutRedirectUris.RemoveRange(clientPostLogoutRedirectUris);
        }
    }
}
