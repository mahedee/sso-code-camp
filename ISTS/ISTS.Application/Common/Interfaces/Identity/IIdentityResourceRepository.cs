using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces.Identity
{
    public interface IIdentityResourceRepository
    {
        Task<IEnumerable<IdentityResource>> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<IdentityResource> GetIdentityResourceAsync(int identityResourceId);
        Task<bool> IsExistIdentityResourceAsync(string name);

        Task<bool> IsExistsIdentityResourceAsync(IdentityResource identityResource);

        Task<int> AddIdentityResourceAsync(IdentityResource identityResource);

        Task<int> UpdateIdentityResourceAsync(IdentityResource identityResource);

        Task<int> DeleteIdentityResourceAsync(IdentityResource identityResource);

        Task<IEnumerable<IdentityResourceProperty>> GetIdentityResourcePropertiesAsync(int identityResourceId,
            int page = 1, int pageSize = 10);

        Task<IdentityResourceProperty> GetIdentityResourcePropertyAsync(int identityResourcePropertyId);

        Task<int> AddIdentityResourcePropertyAsync(int identityResourceId,
            IdentityResourceProperty identityResourceProperty);

        Task<int> DeleteIdentityResourcePropertyAsync(IdentityResourceProperty identityResourceProperty);
    }
}
