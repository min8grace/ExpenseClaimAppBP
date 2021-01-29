using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IRepositoryAsync<Currency> _repository;
        private readonly IDistributedCache _distributedCache;

        public CurrencyRepository(IDistributedCache distributedCache, IRepositoryAsync<Currency> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Currency> Currencys => _repository.Entities;

        public async Task DeleteAsync(Currency currency)
        {
            await _repository.DeleteAsync(currency);
            await _distributedCache.RemoveAsync(CacheKeys.CurrencyCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.CurrencyCacheKeys.GetKey(currency.Id));
        }

        public async Task<Currency> GetByIdAsync(int currencyId)
        {
            return await _repository.Entities.Where(p => p.Id == currencyId).FirstOrDefaultAsync();
        }

        public async Task<List<Currency>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Currency currency)
        {
            await _repository.AddAsync(currency);
            await _distributedCache.RemoveAsync(CacheKeys.CurrencyCacheKeys.ListKey);
            return currency.Id;
        }

        public async Task UpdateAsync(Currency currency)
        {
            await _repository.UpdateAsync(currency);
            await _distributedCache.RemoveAsync(CacheKeys.CurrencyCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.CurrencyCacheKeys.GetKey(currency.Id));
        }
    }
}
