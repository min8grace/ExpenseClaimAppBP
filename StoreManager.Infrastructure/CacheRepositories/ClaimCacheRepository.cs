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
        private readonly IClaimRepository _claimRepository;

        public ClaimCacheRepository(IDistributedCache distributedCache, IClaimRepository claimRepository)
        {
            _distributedCache = distributedCache;
            _claimRepository = claimRepository;
        }

        public async Task<Claim> GetByIdAsync(int claimId)
        {
            string cacheKey = ClaimCacheKeys.GetKey(claimId);
            var claim = await _distributedCache.GetAsync<Claim>(cacheKey);
            if (claim == null)
            {
                claim = await _claimRepository.GetByIdAsync(claimId);
                Throw.Exception.IfNull(claim, "Claim", "No Claim Found");
                await _distributedCache.SetAsync(cacheKey, claim);
            }
            return claim;
        }

        public async Task<List<Claim>> GetCachedListAsync()
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }
            return claimList;
        }
    }
}
