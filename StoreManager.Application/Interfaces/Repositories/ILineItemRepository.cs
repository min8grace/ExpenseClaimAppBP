using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.Repositories
{
    public interface ILineItemRepository
    {
        IQueryable<LineItem> LineItems { get; }

        Task<List<LineItem>> GetListAsync();

        Task<LineItem> GetByIdAsync(int lineItemId);

        Task<int> InsertAsync(LineItem lineItem);

        Task UpdateAsync(LineItem lineItem);

        Task DeleteAsync(LineItem lineItem);
    }
}
