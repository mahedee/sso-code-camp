using IdentityServer4.EntityFramework.Entities;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Extensions;
using ISTS.Application.Common.Interfaces.DbContexts;
using ISTS.Application.Common.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ISTS.Infrastructure.Identity
{
    public class IdentityResourceRepository : IIdentityResourceRepository
    {
        private readonly IAdminConfigurationDbContext _adminConfigurationDbContext;

        public IdentityResourceRepository(IAdminConfigurationDbContext adminConfigurationDbContext)
        {
            _adminConfigurationDbContext = adminConfigurationDbContext;
        }

        public async Task<IEnumerable<IdentityResource>> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            Expression<Func<IdentityResource, bool>> searchCondition = x => x.Name.Contains(search);

            return await _adminConfigurationDbContext.IdentityResources
                .Include(x => x.UserClaims)
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition).PageBy(x => x.Name, page, pageSize).ToListAsync();
        }

        public async Task<IdentityResource> GetIdentityResourceAsync(int identityResourceId)
        {
            return await _adminConfigurationDbContext.IdentityResources
                .Include(x => x.UserClaims)
                .FirstOrDefaultAsync(x => x.Id == identityResourceId);
        }

        public async Task<bool> IsExistIdentityResourceAsync(string name)
        {
            return await _adminConfigurationDbContext.IdentityResources
                .AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExistsIdentityResourceAsync(IdentityResource identityResource)
        {
            if (identityResource.Id == 0)
            {
                var existsWithSameName = await _adminConfigurationDbContext.IdentityResources.Where(x => x.Name == identityResource.Name).FirstOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await _adminConfigurationDbContext.IdentityResources.Where(x => x.Name == identityResource.Name && x.Id != identityResource.Id).FirstOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public async Task<int> AddIdentityResourceAsync(IdentityResource identityResource)
        {
            await _adminConfigurationDbContext.IdentityResources.AddAsync(identityResource);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return identityResource.Id;
        }

        public async Task<int> UpdateIdentityResourceAsync(IdentityResource identityResource)
        {
            //Remove Old relations 

            await RemoveIdentityResourceOldRelations(identityResource);

            //Update with new relations
            _adminConfigurationDbContext.IdentityResources.Update(identityResource);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return identityResource.Id;
        }

        public async Task<int> DeleteIdentityResourceAsync(IdentityResource identityResource)
        {
            _adminConfigurationDbContext.IdentityResources.Remove(identityResource);
            await _adminConfigurationDbContext.SaveChangesAsync();

            return identityResource.Id;
        }

        public async Task<IEnumerable<IdentityResourceProperty>> GetIdentityResourcePropertiesAsync(int identityResourceId, int page = 1, int pageSize = 10)
        {
            return await _adminConfigurationDbContext.IdentityResourceProperties
                .Where(x => x.IdentityResourceId == identityResourceId)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();
        }

        public async Task<IdentityResourceProperty> GetIdentityResourcePropertyAsync(int identityResourcePropertyId)
        {
            return await _adminConfigurationDbContext.IdentityResourceProperties.FirstOrDefaultAsync(x =>
                x.Id == identityResourcePropertyId);
        }

        public async Task<int> AddIdentityResourcePropertyAsync(int identityResourceId, IdentityResourceProperty identityResourceProperty)
        {
            var identityResource = await _adminConfigurationDbContext.IdentityResources.Where(x => x.Id == identityResourceId).FirstOrDefaultAsync();

            identityResourceProperty.IdentityResource = identityResource;
            await _adminConfigurationDbContext.IdentityResourceProperties.AddAsync(identityResourceProperty);

            await _adminConfigurationDbContext.SaveChangesAsync();

            return identityResourceProperty.Id;
        }

        public async Task<int> DeleteIdentityResourcePropertyAsync(IdentityResourceProperty identityResourceProperty)
        {
            var propertyToDelete = await _adminConfigurationDbContext.IdentityResourceProperties.Where(x => x.Id == identityResourceProperty.Id).SingleOrDefaultAsync();

            if (propertyToDelete == null)
            {
                throw new NotFoundException("Identity Resource Property not found");
            }

            _adminConfigurationDbContext.IdentityResourceProperties.Remove(propertyToDelete);

            await _adminConfigurationDbContext.SaveChangesAsync();

            return propertyToDelete.Id;
        }

        private async Task RemoveIdentityResourceOldRelations(IdentityResource resource)
        {
            var userClaims = await _adminConfigurationDbContext.IdentityClaims
                .Where(x => x.IdentityResourceId == resource.Id).ToListAsync();

            _adminConfigurationDbContext.IdentityClaims.RemoveRange(userClaims);

            var identityProperties = await _adminConfigurationDbContext.IdentityResourceProperties
                .Where(x => x.IdentityResourceId == resource.Id).ToListAsync();

            _adminConfigurationDbContext.IdentityResourceProperties.RemoveRange(identityProperties);
        }
    }
}
