using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages
{
    public class ClaimListBase : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public IClaimService ClaimService { get; set; }

        public List<GetAllClaimsResponse> Claims { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public int ClaimId { get; set; }

        protected GlobalUse.Components.ConfirmBase DeleteConfirmation { get; set; }

        protected void Create_Click()
        {
            NavigationManager.NavigateTo("/edit", true);
        }

        protected async Task Delete_Click()
        {
            await ClaimService.DeleteClaim(ClaimId);
            NavigationManager.NavigateTo("/", true);
        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await ClaimService.DeleteClaim(ClaimId);
                NavigationManager.NavigateTo("/", true);
            }
        }

        public string Name { get; set; }
        public string Role { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
            var AuthenticationStateUser = authenticationState.User;
            Name = AuthenticationStateUser.Identity.Name;
            if (Name == null)
            {
                Name = (await authenticationStateTask).User.Identity.Name;
            }

            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/list");
                NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
            }

            if (authenticationState.User.IsInRole("Admin"))
            {
                Role= "Admin";
                Claims = (await ClaimService.GetClaims()).ToList();
            }
            else if (authenticationState.User.IsInRole("Finance"))
            {
                Role = "Finance";
                Claims = (await ClaimService.GetClaims()).ToList().Where(x => x.Status == StoreManager.Domain.Entities.Expense.Status.Approved).ToList();
            }
            else if (authenticationState.User.IsInRole("Approver"))
            {
                Role = "Approver";
                Claims = (await ClaimService.GetClaims()).ToList().Where(x => x.Status == StoreManager.Domain.Entities.Expense.Status.Requested).ToList();
            }
            else if (authenticationState.User.IsInRole("Basic"))
            {
                Role = "Basic";
                Claims = (await ClaimService.GetClaims()).ToList().Where(x => x.Requester.Equals(Name)).ToList();
            }
        }
    }
}
