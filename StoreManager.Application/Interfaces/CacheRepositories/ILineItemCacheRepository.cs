using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.CacheRepositories
{
    public interface ILineItemCacheRepository
    {
        Task<List<LineItem>> GetCachedListAsync();

        Task<LineItem> GetByIdAsync(int brandId);
    }
}
