using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        IQueryable<Currency> Currencys { get; }

        Task<List<Currency>> GetListAsync();

        Task<Currency> GetByIdAsync(int currencyId);

        Task<int> InsertAsync(Currency currency);

        Task UpdateAsync(Currency currency);

        Task DeleteAsync(Currency currency);


    }
}
