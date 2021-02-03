using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public interface ICurrencyService
    {
        Task<List<GetAllCurrenciesResponse>> GetCurrencies();

        Task<Currency> GetCurrencyById(int id);

        Task<Currency> CreateCurrency(Currency newCurrency);

        Task UpdateCurrency(Currency updatedCurrency);

        Task DeleteCurrency(int id);
    }
}
