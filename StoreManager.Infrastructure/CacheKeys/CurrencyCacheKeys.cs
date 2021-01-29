using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
namespace StoreManager.Infrastructure.CacheKeys
{
    public static class CurrencyCacheKeys
    {
        public static string ListKey => "CurrencyList";

        public static string SelectListKey => "CurrencySelectList";

        public static string GetKey(int currencyId) => $"Currency-{currencyId}";

        public static string GetDetailsKey(int currencyId) => $"CurrencyDetails-{currencyId}";
    }
}
