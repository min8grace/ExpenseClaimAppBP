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
    public class LineItemCacheRepository : ILineItemCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILineItemRepository _lineItemRepository;

        public LineItemCacheRepository(IDistributedCache distributedCache, ILineItemRepository lineItemRepository)
        {
            _distributedCache = distributedCache;
            _lineItemRepository = lineItemRepository;
        }

        public async Task<LineItem> GetByIdAsync(int lineItemId)
        {
            string cacheKey = LineItemCacheKeys.GetKey(lineItemId);
            var lineItem = await _distributedCache.GetAsync<LineItem>(cacheKey);
            if (lineItem == null)
            {
                lineItem = await _lineItemRepository.GetByIdAsync(lineItemId);
                Throw.Exception.IfNull(lineItem, "LineItem", "No LineItem Found");
                await _distributedCache.SetAsync(cacheKey, lineItem);
            }
            return lineItem;
        }

        public async Task<List<LineItem>> GetCachedListAsync()
        {
            string cacheKey = LineItemCacheKeys.ListKey;
            var lineItemList = await _distributedCache.GetAsync<List<LineItem>>(cacheKey);
            if (lineItemList == null)
            {
                lineItemList = await _lineItemRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, lineItemList);
            }
            return lineItemList;
        }
    }
}
