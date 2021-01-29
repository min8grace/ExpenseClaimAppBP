using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.CacheRepositories
{
    public interface ICategoryCacheRepository
    {
        Task<List<Category>> GetCachedListAsync();

        Task<Category> GetByIdAsync(int categoryId);
    }
}
