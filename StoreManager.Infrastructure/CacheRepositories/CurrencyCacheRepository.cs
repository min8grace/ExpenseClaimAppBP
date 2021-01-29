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
    public class CurrencyCacheRepository : ICurrencyCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyCacheRepository(IDistributedCache distributedCache, ICurrencyRepository currencyRepository)
        {
            _distributedCache = distributedCache;
            _currencyRepository = currencyRepository;
        }

        public async Task<Currency> GetByIdAsync(int currencyId)
        {
            string cacheKey = CurrencyCacheKeys.GetKey(currencyId);
            var currency = await _distributedCache.GetAsync<Currency>(cacheKey);
            if (currency == null)
            {
                currency = await _currencyRepository.GetByIdAsync(currencyId);
                Throw.Exception.IfNull(currency, "Currency", "No Currency Found");
                await _distributedCache.SetAsync(cacheKey, currency);
            }
            return currency;
        }

        public async Task<List<Currency>> GetCachedListAsync()
        {
            string cacheKey = CurrencyCacheKeys.ListKey;
            var currencyList = await _distributedCache.GetAsync<List<Currency>>(cacheKey);
            if (currencyList == null)
            {
                currencyList = await _currencyRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, currencyList);
            }
            return currencyList;
        }
    }
}
