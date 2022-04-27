using IdentityServer4.EntityFramework.Entities;
using ISTS.Application.Common.Extensions;
using ISTS.Application.Common.Interfaces.DbContexts;
using ISTS.Application.Common.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ISTS.Infrastructure.Identity
{
    public class ApiScopeRepository : IApiScopeRepository
    {
        private readonly IAdminConfigurationDbContext _adminConfigurationDbContext;

        public ApiScopeRepository(IAdminConfigurationDbContext adminConfigurationDbContext)
        {
            _adminConfigurationDbContext = adminConfigurationDbContext;
        }

        public virtual Task<ApiScopeProperty> GetApiScopePropertyAsync(int apiScopePropertyId)
        {
            return _adminConfigurationDbContext.ApiScopeProperties
                .Include(x => x.Scope)
                .Where(x => x.Id == apiScopePropertyId)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<int> AddApiScopePropertyAsync(int apiScopeId, ApiScopeProperty apiScopeProperty)
        {
            var apiScope = await _adminConfigurationDbContext.ApiScopes.Where(x => x.Id == apiScopeId).SingleOrDefaultAsync();

            apiScopeProperty.Scope = apiScope;
            await _adminConfigurationDbContext.ApiScopeProperties.AddAsync(apiScopeProperty);

            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteApiScopePropertyAsync(ApiScopeProperty apiScopeProperty)
        {
            var propertyToDelete = await _adminConfigurationDbContext.ApiScopeProperties.Where(x => x.Id == apiScopeProperty.Id).SingleOrDefaultAsync();

            _adminConfigurationDbContext.ApiScopeProperties.Remove(propertyToDelete);

            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public virtual async Task<bool> CanInsertApiScopePropertyAsync(ApiScopeProperty apiScopeProperty)
        {
            var existsWithSameName = await _adminConfigurationDbContext.ApiScopeProperties.Where(x => x.Key == apiScopeProperty.Key
                                                                                   && x.Scope.Id == apiScopeProperty.Scope.Id).SingleOrDefaultAsync();
            return existsWithSameName == null;
        }


        public virtual async Task<string> GetApiScopeNameAsync(int apiScopeId)
        {
            var apiScopeName = await _adminConfigurationDbContext.ApiScopes.Where(x => x.Id == apiScopeId).Select(x => x.Name).SingleOrDefaultAsync();

            return apiScopeName;
        }

        public virtual async Task<IEnumerable<ApiScopeProperty>> GetApiScopePropertiesAsync(int apiScopeId, int page = 1, int pageSize = 10)
        {

            var apiScopeProperties = _adminConfigurationDbContext.ApiScopeProperties.Where(x => x.Scope.Id == apiScopeId);

            var properties = await apiScopeProperties.PageBy(x => x.Id, page, pageSize)
                .ToListAsync();


            return properties;
        }

        public virtual async Task<bool> CanInsertApiScopeAsync(ApiScope apiScope)
        {
            if (apiScope.Id == 0)
            {
                var existsWithSameName = await _adminConfigurationDbContext.ApiScopes.Where(x => x.Name == apiScope.Name).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await _adminConfigurationDbContext.ApiScopes.Where(x => x.Name == apiScope.Name && x.Id != apiScope.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public virtual async Task<IEnumerable<ApiScope>> GetApiScopesAsync(string search, int page = 1, int pageSize = 10)
        {
            Expression<Func<ApiScope, bool>> searchCondition = x => x.Name.Contains(search);

            var filteredApiScopes = _adminConfigurationDbContext.ApiScopes
                .Include(x => x.UserClaims)
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition);

            var apiScopes = await filteredApiScopes
                .PageBy(x => x.Name, page, pageSize).ToListAsync();

            return apiScopes;
        }

        public virtual async Task<ICollection<string>> GetApiScopesNameAsync(string scope, int limit = 0)
        {
            var apiScopes = await _adminConfigurationDbContext.ApiScopes
                .WhereIf(!string.IsNullOrEmpty(scope), x => x.Name.Contains(scope))
                .TakeIf(x => x.Id, limit > 0, limit)
                .Select(x => x.Name).ToListAsync();

            return apiScopes;
        }

        public async Task<ApiScope> GetApiScopeAsync(int apiScopeId)
        {
            return await _adminConfigurationDbContext.ApiScopes
                .Include(x => x.UserClaims)
                .Where(x => x.Id == apiScopeId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<ApiScope> GetApiScopeAsync(string name)
        {
            return await _adminConfigurationDbContext.ApiScopes
                .Include(x => x.UserClaims)
                .Where(x => x.Name == name)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Add new api scope
        /// </summary>
        /// <param name="apiScope"></param>
        /// <returns>This method return new api scope id</returns>
        public virtual async Task<int> AddApiScopeAsync(ApiScope apiScope)
        {
            await _adminConfigurationDbContext.ApiScopes.AddAsync(apiScope);

            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiScope.Id;
        }

        private async Task RemoveApiScopeClaimsAsync(ApiScope apiScope)
        {
            //Remove old api scope claims
            var apiScopeClaims = await _adminConfigurationDbContext.ApiScopeClaims.Where(x => x.Scope.Id == apiScope.Id).ToListAsync();
            _adminConfigurationDbContext.ApiScopeClaims.RemoveRange(apiScopeClaims);
        }

        public virtual async Task<int> UpdateApiScopeAsync(ApiScope apiScope)
        {
            //Remove old relations
            await RemoveApiScopeClaimsAsync(apiScope);

            //Update with new data
            _adminConfigurationDbContext.ApiScopes.Update(apiScope);

            return await _adminConfigurationDbContext.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteApiScopeAsync(ApiScope apiScope)
        {
            var apiScopeToDelete = await _adminConfigurationDbContext.ApiScopes.Where(x => x.Id == apiScope.Id).SingleOrDefaultAsync();
            _adminConfigurationDbContext.ApiScopes.Remove(apiScopeToDelete);

            return await _adminConfigurationDbContext.SaveChangesAsync();
        }
    }
}
