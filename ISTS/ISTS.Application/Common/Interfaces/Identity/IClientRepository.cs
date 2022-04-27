using IdentityServer4.EntityFramework.Entities;
using ISTS.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IClientRepository
    {
        Task<int> AddClientAsync(Client client);

        Task<int> AddClientDetailsAsync(ClientDetails? clientDetails);
        Task<int> UpdateClientAsync(Client client);

        Task<ClientDetails?> GetClientAdditionalDetailsAsync(int clientId);

        Task<int> RemoveClientAsync(Client client);

        Task<Client?> GetClientAsync(int clientId);
        Task<Client?> GetClientAsync(string clientId);
        Task<bool> IsClientAlreadyExists(Client client);
        Task<(string? ClientId, string? ClientName)> GetClientIdAsync(int clientId);

        Task<IEnumerable<Client>> GetClientsAsync(string search = "", int page = 1, int pageSize = 10);

        Task<List<string>> GetScopesAsync(string scope, int limit = 0);

        List<string> GetGrantTypes(string grant, int limit = 0);

        List<SelectItem> GetProtocolTypes();

        List<SelectItem> GetAccessTokenTypes();

        List<SelectItem> GetTokenExpirations();

        List<SelectItem> GetTokenUsage();

        List<SelectItem> GetHashTypes();

        List<SelectItem> GetSecretTypes();

        List<string> GetStandardClaims(string claim, int limit = 0);

        Task<int> AddClientSecretAsync(int clientId, ClientSecret clientSecret);

        Task<int> DeleteClientSecretAsync(ClientSecret clientSecret);

        Task<IEnumerable<ClientSecret>> GetClientSecretsAsync(int clientId, int page = 1, int pageSize = 10);

        Task<ClientSecret> GetClientSecretAsync(int clientId, int clientSecretId);

        Task<IEnumerable<ClientClaim>> GetClientClaimsAsync(int clientId, int page = 1, int pageSize = 10);

        Task<IEnumerable<ClientProperty>> GetClientPropertiesAsync(int clientId, int page = 1, int pageSize = 10);

        Task<ClientClaim> GetClientClaimAsync(int clientClaimId);

        Task<ClientProperty> GetClientPropertyAsync(int clientPropertyId);

        Task<int> AddClientClaimAsync(int clientId, ClientClaim clientClaim);

        Task<int> AddClientPropertyAsync(int clientId, ClientProperty clientProperties);

        Task<int> DeleteClientClaimAsync(ClientClaim clientClaim);

        Task<int> DeleteClientPropertyAsync(ClientProperty clientProperty);

        List<string> GetSigningAlgorithms(string algorithm, int limit = 0);
    }
}
