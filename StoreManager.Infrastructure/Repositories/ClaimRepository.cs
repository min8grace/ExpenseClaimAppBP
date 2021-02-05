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

        public async Task DeleteAsync(Claim claim)
        {
            await _repository.DeleteAsync(claim);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.GetKey(claim.Id));
        }

        public async Task<Claim> GetByIdAsync(int claimId)
        {
            //return await _repository.Entities.Where(p => p.Id == claimId).FirstOrDefaultAsync();
            var result = await _repository.Entities.Where(c => c.Id == claimId)
                .Include(c => c.LineItems)
                    .ThenInclude(c => c.Category)
                .Include(c => c.LineItems)
                    .ThenInclude(c => c.Currency)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Claim>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Claim claim)
        {
            await _repository.AddAsync(claim);
            //await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.ListKey);
            return claim.Id;
        }

        public async Task UpdateAsync(Claim claim)
        {
            await _repository.UpdateAsync(claim);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.ClaimCacheKeys.GetKey(claim.Id));
        }
    }
}
