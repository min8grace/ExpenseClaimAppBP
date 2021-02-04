using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Domain.Entities.Expense;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ExpenseClaimApp.Pages.LineItems
{
    public class LineItemDetailBase : ComponentBase
    {
        [Inject]
        public ILineItemService LineItemService { get; set; }

        public LineItem LineItem { get; set; }

        public IEnumerable<LineItem> LineItems { get; set; }

        [Parameter]
        public string Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Id = Id ?? "1";
            LineItem = await LineItemService.GetLineItemById(int.Parse(Id));
        }
    }
}
