using StoreManager.Application.Interfaces.CacheRepositories;
using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using StoreManager.Infrastructure.CacheKeys;
using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Infrastructure.CacheRepositories
{
    public class ClaimCacheRepository : IClaimCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IClaimRepository _brandRepository;

        public ClaimCacheRepository(IDistributedCache distributedCache, IClaimRepository brandRepository)
        {
            _distributedCache = distributedCache;
            _brandRepository = brandRepository;
        }

        public async Task<Claim> GetByIdAsync(int brandId)
        {
            string cacheKey = ClaimCacheKeys.GetKey(brandId);
            var brand = await _distributedCache.GetAsync<Claim>(cacheKey);
            if (brand == null)
            {
                brand = await _brandRepository.GetByIdAsync(brandId);
                Throw.Exception.IfNull(brand, "Claim", "No Claim Found");
                await _distributedCache.SetAsync(cacheKey, brand);
            }
            return brand;
        }

        public async Task<List<Claim>> GetCachedListAsync()
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var brandList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (brandList == null)
            {
                brandList = await _brandRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, brandList);
            }
            return brandList;
        }
    }
}
