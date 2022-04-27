using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using ISTS.Application.Common.Interfaces.DbContexts;
using ISTS.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure.DbContexts
{
    public class IdentityServerConfigurationDbContext : ConfigurationDbContext<IdentityServerConfigurationDbContext>, IAdminConfigurationDbContext
    {
        public IdentityServerConfigurationDbContext(DbContextOptions<IdentityServerConfigurationDbContext> options, ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }


        /// <summary>
        /// Get API Resource Secrets
        /// </summary>
        public DbSet<ApiResourceSecret> ApiSecrets { get; set; }

        /// <summary>
        /// Get API Scope Claims
        /// </summary>
        public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        /// <summary>
        /// Get identity Resource Claims
        /// </summary>
        public DbSet<IdentityResourceClaim> IdentityClaims { get; set; }

        /// <summary>
        /// Get API Resource Claims
        /// </summary>
        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        /// <summary>
        /// Get Client Grant Types
        /// </summary>
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }

        /// <summary>
        /// Get Client Scopes
        /// </summary>
        public DbSet<ClientScope> ClientScopes { get; set; }

        /// <summary>
        /// Get Client Secrets
        /// </summary>
        public DbSet<ClientSecret> ClientSecrets { get; set; }

        /// <summary>
        /// Get Client Post Logout Uris
        /// </summary>
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }

        /// <summary>
        /// Get Client IdP Restrictions
        /// </summary>
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }

        /// <summary>
        /// Get Client Redirect Uris
        /// </summary>
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }

        /// <summary>
        /// Get Client Claims
        /// </summary>
        public DbSet<ClientClaim> ClientClaims { get; set; }

        /// <summary>
        /// Get Client Properties
        /// </summary>
        public DbSet<ClientProperty> ClientProperties { get; set; }

        /// <summary>
        /// Get Identity Resource properties
        /// </summary>
        public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }

        /// <summary>
        /// Get API resource Properties
        /// </summary>
        public DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }

        /// <summary>
        /// Get API Scope Properties
        /// </summary>
        public DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }

        /// <summary>
        /// Get API Resource Scopes
        /// </summary>
        public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }

        /// <summary>
        /// Get Additional Client Details information
        /// </summary>
        public DbSet<ClientDetails?> ClientDetails { get; set; }

        /// <summary>
        /// Save Changes data
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            //Can track entity state
            //Can add audit trail
            return await base.SaveChangesAsync();
        }
    }
}
