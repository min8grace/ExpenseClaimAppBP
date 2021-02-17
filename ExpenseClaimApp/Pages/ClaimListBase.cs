using AutoMapper;
using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using System;
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
        [Inject]
        public IMapper Mapper { get; set; }
        public List<GetAllClaimsResponse> Claims { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public int ClaimId { get; set; }

        protected GlobalUse.Components.ConfirmBase DeleteConfirmation { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        protected string titleFilter = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await LoadClaim();
        }

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
        protected async Task Filter()
        {
            //currentPage = 1;
            await LoadClaim();
        }

        protected async Task Clear()
        {
            titleFilter = string.Empty;
            //currentPage = 1;
            await LoadClaim();
        }

        //async Task LoadClaim(int page = 1, int quantityPerPage = 10)
        protected async Task LoadClaim()
        {
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

            if (authenticationState.User.IsInRole("Admin"))
            {
                   Role = "Admin";
                if (titleFilter != string.Empty) { Claims = (await ClaimService.GetSearchClaims(titleFilter)).ToList(); }
                else { Claims = (await ClaimService.GetClaims()).ToList(); } 
            }
            else if (authenticationState.User.IsInRole("Finance"))
            {
                Role = "Finance";
                if (titleFilter != string.Empty) { Claims = (await ClaimService.GetSearchClaims(titleFilter)).ToList().Where(x => x.Status == StoreManager.Domain.Entities.Expense.Status.Approved).ToList(); }
                else { Claims = (await ClaimService.GetClaims()).ToList().Where(x => x.Status == StoreManager.Domain.Entities.Expense.Status.Approved).ToList(); }
                
            }
            else if (authenticationState.User.IsInRole("Approver"))
            {
                Role = "Approver";
                if (titleFilter != string.Empty) { Claims = (await ClaimService.GetSearchClaims(titleFilter)).ToList().Where(x => x.Status == StoreManager.Domain.Entities.Expense.Status.Requested).ToList(); }
                else { Claims = (await ClaimService.GetClaims()).ToList().Where(x => x.Status == StoreManager.Domain.Entities.Expense.Status.Requested).ToList(); }               
            }
            else if (authenticationState.User.IsInRole("Basic"))
            {
                Role = "Basic";
                if (titleFilter != string.Empty) { Claims = (await ClaimService.GetSearchClaims(titleFilter)).ToList().Where(x => x.Requester.Equals(Name)).ToList(); }
                else { Claims = (await ClaimService.GetClaims()).ToList().Where(x => x.Requester.Equals(Name)).ToList(); }
            }

            foreach (var claim in AuthenticationStateUser.Claims)
            {
                Console.WriteLine(claim.Type);
                Console.WriteLine(claim.Value);
            }
        }
    }
}
