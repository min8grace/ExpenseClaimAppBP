using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.CacheRepositories
{
    public interface ICurrencyCacheRepository
    {
        Task<List<Currency>> GetCachedListAsync();

        Task<Currency> GetByIdAsync(int currencyId);
    }
}
