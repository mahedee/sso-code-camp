using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IApiScopeRepository
    {
        Task<IEnumerable<ApiScope>> GetApiScopesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiScope> GetApiScopeAsync(int apiScopeId);
        Task<ApiScope> GetApiScopeAsync(string name);

        Task<int> AddApiScopeAsync(ApiScope apiScope);

        Task<int> UpdateApiScopeAsync(ApiScope apiScope);

        Task<int> DeleteApiScopeAsync(ApiScope apiScope);

        Task<bool> CanInsertApiScopeAsync(ApiScope apiScope);

        Task<ICollection<string>> GetApiScopesNameAsync(string scope, int limit = 0);

        Task<IEnumerable<ApiScopeProperty>> GetApiScopePropertiesAsync(int apiScopeId, int page = 1, int pageSize = 10);

        Task<ApiScopeProperty> GetApiScopePropertyAsync(int apiScopePropertyId);

        Task<int> AddApiScopePropertyAsync(int apiScopeId, ApiScopeProperty apiScopeProperty);

        Task<bool> CanInsertApiScopePropertyAsync(ApiScopeProperty apiScopeProperty);

        Task<int> DeleteApiScopePropertyAsync(ApiScopeProperty apiScopeProperty);

        Task<string> GetApiScopeNameAsync(int apiScopeId);
    }
}
