using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Infrastructure.Repositories
{
    public class LineItemRepository : ILineItemRepository
    {
        private readonly IRepositoryAsync<LineItem> _repository;
        private readonly IDistributedCache _distributedCache;

        public LineItemRepository(IDistributedCache distributedCache, IRepositoryAsync<LineItem> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<LineItem> LineItems => _repository.Entities;

        public async Task DeleteAsync(LineItem lineItem)
        {
            await _repository.DeleteAsync(lineItem);
            await _distributedCache.RemoveAsync(CacheKeys.LineItemCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.LineItemCacheKeys.GetKey(lineItem.Id));
        }

        public async Task<LineItem> GetByIdAsync(int lineItemId)
        {
            return await _repository.Entities.Where(p => p.Id == lineItemId)
                .Include(x => x.Currency)
                .Include(x => x.Category).FirstOrDefaultAsync();
        }

        public async Task<List<LineItem>> GetListAsync()
        {
            return await _repository.Entities
                .Include(x=>x.Currency)
                .Include(x => x.Category).ToListAsync();
        }

        public async Task<int> InsertAsync(LineItem lineItem)
        {
            await _repository.AddAsync(lineItem);
            await _distributedCache.RemoveAsync(CacheKeys.LineItemCacheKeys.ListKey);
            return lineItem.Id;
        }

        public async Task UpdateAsync(LineItem lineItem)
        {
            await _repository.UpdateAsync(lineItem);
            await _distributedCache.RemoveAsync(CacheKeys.LineItemCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.LineItemCacheKeys.GetKey(lineItem.Id));
        }
    }
}
