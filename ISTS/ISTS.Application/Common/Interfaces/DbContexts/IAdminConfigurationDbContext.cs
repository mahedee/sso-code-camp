using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using ISTS.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ISTS.Application.Common.Interfaces.DbContexts
{
    public interface IAdminConfigurationDbContext : IConfigurationDbContext
    {
        /// <summary>
        /// Get API Resource Secrets
        /// </summary>
        DbSet<ApiResourceSecret> ApiSecrets { get; set; }
        /// <summary>
        /// Get API Scope Claims
        /// </summary>

        DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
        /// <summary>
        /// Get identity Resource Claims
        /// </summary>
        DbSet<IdentityResourceClaim> IdentityClaims { get; set; }
        /// <summary>
        /// Get API Resource Claims
        /// </summary>
        DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
        /// <summary>
        /// Get Client Grant Types
        /// </summary>
        DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        /// <summary>
        /// Get Client Scopes
        /// </summary>
        DbSet<ClientScope> ClientScopes { get; set; }
        /// <summary>
        /// Get Client Secrets
        /// </summary>
        DbSet<ClientSecret> ClientSecrets { get; set; }
        /// <summary>
        /// Get Client Post Logout Uris
        /// </summary>
        DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        /// <summary>
        /// Get Client IdP Restrictions
        /// </summary>
        DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        /// <summary>
        /// Get Client Redirect Uris
        /// </summary>
        DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        /// <summary>
        /// Get Client Claims
        /// </summary>
        DbSet<ClientClaim> ClientClaims { get; set; }
        /// <summary>
        /// Get Client Properties
        /// </summary>

        DbSet<ClientProperty> ClientProperties { get; set; }
        /// <summary>
        /// Get Identity Resource properties
        /// </summary>
        DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }
        /// <summary>
        /// Get API resource Properties
        /// </summary>
        DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }
        /// <summary>
        /// Get API Scope Properties
        /// </summary>
        DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }
        /// <summary>
        /// Get API Resource Scopes
        /// </summary>
        DbSet<ApiResourceScope> ApiResourceScopes { get; set; }
        /// <summary>
        /// Get Additional Client Details information
        /// </summary>
        DbSet<ClientDetails?> ClientDetails { get; set; }
        /// <summary>
        /// Save Changes data
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
