using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IApiResourceRepository
    {
        Task<IEnumerable<ApiResource>> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiResource?> GetApiResourceAsync(int apiResourceId);
        Task<ApiResource?> GetApiResourceByNameAsync(string name);

        Task<IEnumerable<ApiResourceProperty>> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10);

        Task<ApiResourceProperty> GetApiResourcePropertyAsync(int apiResourcePropertyId);

        Task<int> AddApiResourcePropertyAsync(int apiResourceId, ApiResourceProperty apiResourceProperty);

        Task<int> DeleteApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty);

        Task<bool> CanInsertApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty);

        Task<int> AddApiResourceAsync(ApiResource apiResource);

        Task<int> UpdateApiResourceAsync(ApiResource apiResource);

        Task<int> DeleteApiResourceAsync(ApiResource apiResource);

        Task<bool> CanInsertApiResourceAsync(ApiResource apiResource);

        Task<IEnumerable<ApiResourceSecret>> GetApiSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10);

        Task<int> AddApiSecretAsync(int apiResourceId, ApiResourceSecret apiSecret);

        Task<ApiResourceSecret> GetApiSecretAsync(int apiSecretId);

        Task<int> DeleteApiSecretAsync(ApiResourceSecret apiSecret);


        Task<string> GetApiResourceNameAsync(int apiResourceId);
    }
}
