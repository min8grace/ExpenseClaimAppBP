using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages
{
    public class ClaimDetailBase : ComponentBase
    {
        [Inject]
        public IClaimService ClaimService { get; set; }

        public Claim Claim { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        [Inject]
        public ILineItemService LineItemService { get; set; }

        [Parameter]
        public string Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Claim = await ClaimService.GetClaimById(int.Parse(Id));

            var authenticationState = await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
            var AuthenticationStateUser = authenticationState.User;
            Name = AuthenticationStateUser.Claims.Where(x => x.Type.Equals("email")).FirstOrDefault().Value;
            if (Name == null)
            {
                Name = (await authenticationStateTask).User.Claims.Where(x => x.Type.Equals("email")).FirstOrDefault().Value;
            }

            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/list");
                NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
            }

            if (authenticationState.User.IsInRole("Admin")) Role = "Admin";
            else if (authenticationState.User.IsInRole("Finance")) Role = "Finance";
            else if (authenticationState.User.IsInRole("Approver")) Role = "Approver";
            else if (authenticationState.User.IsInRole("Basic")) Role = "Basic";
        }
        protected async Task Delete_Click(int SelectedId)
        {
            await LineItemService.DeleteLineItem(SelectedId);
            NavigationManager.NavigateTo("/ins/LineItem", true);
        }

        protected async Task BackToList()
        {
            NavigationManager.NavigateTo("/list", true);
        }
    }
}
