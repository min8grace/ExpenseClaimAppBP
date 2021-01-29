using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Infrastructure.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly IRepositoryAsync<Claim> _repository;
        private readonly IDistributedCache _distributedCache;

        public ClaimRepository(IDistributedCache distributedCache, IRepositoryAsync<Claim> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Claim> Claims => _repository.Entities;

        public async Task DeleteAsync(Claim brand)
        {
            await _repository.DeleteAsync(brand);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.GetKey(brand.Id));
        }

        public async Task<Claim> GetByIdAsync(int brandId)
        {
            return await _repository.Entities.Where(p => p.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<List<Claim>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Claim brand)
        {
            await _repository.AddAsync(brand);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.ListKey);
            return brand.Id;
        }

        public async Task UpdateAsync(Claim brand)
        {
            await _repository.UpdateAsync(brand);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.GetKey(brand.Id));
        }
    }
}
