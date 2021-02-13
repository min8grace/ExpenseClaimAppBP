using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.Processes
{
    public class DisplayClaimDetailBase : ComponentBase
    {

        public IClaimService ClaimService { get; set; }

        [Parameter]
        public Claim Claim { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        [Parameter]
        public string Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected async Task BackToList()
        {
            NavigationManager.NavigateTo("/list", true);
        }
    }
}
