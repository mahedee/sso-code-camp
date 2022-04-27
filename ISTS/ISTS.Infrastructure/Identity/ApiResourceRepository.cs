using IdentityServer4.EntityFramework.Entities;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.DbContexts;
using ISTS.Application.Common.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Infrastructure.Identity
{
    public class ApiResourceRepository : IApiResourceRepository
    {
        private readonly IAdminConfigurationDbContext _adminConfigurationDbContext;

        public ApiResourceRepository(IAdminConfigurationDbContext adminConfigurationDbContext)
        {
            _adminConfigurationDbContext = adminConfigurationDbContext;
        }

        public async Task<IEnumerable<ApiResource>> GetApiResourcesAsync(string search, int page = 1, int pageSize = 1000)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await _adminConfigurationDbContext.ApiResources
                    .Include(x => x.UserClaims)
                    .Include(x => x.Scopes)
                    .Where(x => x.Name.ToLower().Contains(search.ToLower()))
                    .ToListAsync();
            }
            return await _adminConfigurationDbContext.ApiResources
                .Include(x => x.UserClaims)
                .Include(x => x.Scopes).ToListAsync();
        }

        public async Task<ApiResource?> GetApiResourceAsync(int apiResourceId)
        {
            return await _adminConfigurationDbContext.ApiResources
                .Include(x => x.UserClaims)
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.Id == apiResourceId);
        }

        public async Task<ApiResource?> GetApiResourceByNameAsync(string name)
        {
            return await _adminConfigurationDbContext.ApiResources
                .Include(x => x.UserClaims)
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<ApiResourceProperty>> GetApiResourcePropertiesAsync(int apiResourceId, int page = 1, int pageSize = 10)
        {
            return await _adminConfigurationDbContext.ApiResourceProperties.Where(x => x.ApiResourceId == apiResourceId)
                .ToListAsync();
        }

        public async Task<ApiResourceProperty> GetApiResourcePropertyAsync(int apiResourcePropertyId)
        {
            return await _adminConfigurationDbContext.ApiResourceProperties.FirstOrDefaultAsync(x =>
                x.Id == apiResourcePropertyId);
        }

        public async Task<int> AddApiResourcePropertyAsync(int apiResourceId, ApiResourceProperty apiResourceProperty)
        {
            var apiResource =
                await _adminConfigurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Id == apiResourceId);
            if (apiResource == null)
            {
                throw new NotFoundException("Api Resource information not found");
            }

            apiResourceProperty.ApiResourceId = apiResourceId;
            apiResourceProperty.ApiResource = apiResource;

            await _adminConfigurationDbContext.ApiResourceProperties.AddAsync(apiResourceProperty);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiResourceProperty.Id;
        }

        public async Task<int> DeleteApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty)
        {
            _adminConfigurationDbContext.ApiResourceProperties.Remove(apiResourceProperty);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiResourceProperty.Id;
        }

        public async Task<bool> CanInsertApiResourcePropertyAsync(ApiResourceProperty apiResourceProperty)
        {
            var existsWithSameName = await _adminConfigurationDbContext.ApiResourceProperties.Where(x => x.Key == apiResourceProperty.Key
                && x.ApiResource.Id == apiResourceProperty.ApiResourceId).FirstOrDefaultAsync();
            return existsWithSameName == null;
        }

        public async Task<int> AddApiResourceAsync(ApiResource apiResource)
        {
            await _adminConfigurationDbContext.ApiResources.AddAsync(apiResource);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiResource.Id;
        }

        public async Task<int> UpdateApiResourceAsync(ApiResource apiResource)
        {
            //Remove Old relationship
            await RemoveOldRelationApiResource(apiResource);

            //Update api resource
            _adminConfigurationDbContext.ApiResources.Update(apiResource);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiResource.Id;
        }

        private async Task RemoveOldRelationApiResource(ApiResource apiResource)
        {
            var apiResourceProperties =
                await _adminConfigurationDbContext.ApiResourceProperties.Where(x => x.ApiResourceId == apiResource.Id).ToListAsync();
            _adminConfigurationDbContext.ApiResourceProperties.RemoveRange(apiResourceProperties);

            var apiResourceClaims = await _adminConfigurationDbContext.ApiResourceClaims
                .Where(x => x.ApiResourceId == apiResource.Id).ToListAsync();

            _adminConfigurationDbContext.ApiResourceClaims.RemoveRange(apiResourceClaims);

            var apiResourceScopes = await _adminConfigurationDbContext.ApiResourceScopes
                .Where(x => x.ApiResourceId == apiResource.Id).ToListAsync();
            _adminConfigurationDbContext.ApiResourceScopes.RemoveRange(apiResourceScopes);
        }

        public async Task<int> DeleteApiResourceAsync(ApiResource apiResource)
        {
            _adminConfigurationDbContext.ApiResources.Remove(apiResource);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiResource.Id;
        }

        public async Task<bool> CanInsertApiResourceAsync(ApiResource apiResource)
        {
            if (apiResource.Id == 0)
            {
                var existsWithSameName = await _adminConfigurationDbContext.ApiResources.Where(x => x.Name == apiResource.Name).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await _adminConfigurationDbContext.ApiResources.Where(x => x.Name == apiResource.Name && x.Id != apiResource.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public async Task<IEnumerable<ApiResourceSecret>> GetApiSecretsAsync(int apiResourceId, int page = 1, int pageSize = 10)
        {
            var apiResource = await _adminConfigurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                throw new NotFoundException("API resource information not found");
            }

            var data = await _adminConfigurationDbContext
                .ApiSecrets
                .Where(x => x.ApiResourceId == apiResourceId)
                .Include(x => x.ApiResource)
                .ToListAsync();

            data.ForEach(x => x.Value = null);
            return data;
        }

        public async Task<int> AddApiSecretAsync(int apiResourceId, ApiResourceSecret apiSecret)
        {
            var apiResource =
                await _adminConfigurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Id == apiResourceId);

            if (apiResource == null)
            {
                throw new NotFoundException("Api Resource information not found");
            }
            apiSecret.ApiResourceId = apiResourceId;
            apiSecret.ApiResource = apiResource;

            await _adminConfigurationDbContext.ApiSecrets.AddAsync(apiSecret);

            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiSecret.Id;
        }

        public async Task<ApiResourceSecret> GetApiSecretAsync(int apiSecretId)
        {
            return await _adminConfigurationDbContext.ApiSecrets.FirstOrDefaultAsync(x => x.Id == apiSecretId);
        }

        public async Task<int> DeleteApiSecretAsync(ApiResourceSecret apiSecret)
        {
            _adminConfigurationDbContext.ApiSecrets.Remove(apiSecret);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return apiSecret.Id;
        }

        public async Task<string> GetApiResourceNameAsync(int apiResourceId)
        {
            var resource = await _adminConfigurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Id == apiResourceId);
            if (resource == null)
            {
                throw new NotFoundException("Api Resource Information not found");
            }

            return resource.Name;
        }
    }
}
