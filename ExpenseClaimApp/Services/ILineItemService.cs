using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public interface ILineItemService
    {
        Task<List<GetAllLineItemsResponse>> GetLineItems();

        Task<LineItem> GetLineItemById(int id);

        Task<LineItem> CreateLineItem(LineItem newLineItem);

        Task UpdateLineItem(LineItem updatedLineItem);

        Task DeleteLineItem(int id);
    }
}
