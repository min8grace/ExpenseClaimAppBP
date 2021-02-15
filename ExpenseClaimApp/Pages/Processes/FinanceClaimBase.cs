using AutoMapper;
using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.Processes
{
    public class FinanceClaimBase : ComponentBase
    {
        [Inject]
        public IClaimService ClaimService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }
        public Claim Claim { get; set; }
        public ClaimEditModel ClaimEditModel { get; set; } = new ClaimEditModel();
        public IEnumerable<Claim> Claims { get; set; }
        public enum StatusFinance
        {
            RejectedByFinance = 7, Finished = 8
        }
        public StatusFinance SelectedStatus { get; set; }
        public string FinanceComments { get; set; } = string.Empty;
        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        [Parameter]
        public string Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        protected string Name { get; set; }

        protected override async Task OnInitializedAsync()
        {

            Claim = await ClaimService.GetClaimById(int.Parse(Id));
            FinanceComments = Claim.FinanceComments;

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
        }

        protected async Task BackToList()
        {
            NavigationManager.NavigateTo("/list", true);
        }
        protected async Task HandleValidSubmit()
        {
            //Mapper.Map(ClaimEditModel, Claim);       
            Claim.FinanceComments = FinanceComments;
            Claim.ProcessedDate = DateTime.Now;
            Claim.Approver = Name;//Finance Id(email)
            Claim.Status = (Status)SelectedStatus;
            await ClaimService.UpdateClaim(Claim);



            await ClaimService.UpdateClaim(Claim);

            StatusClass = "alert-success";
            Message = "Employee updated successfully.";
            Saved = true;
            //StateHasChanged();
            NavigationManager.NavigateTo("/list", true);
        }
    }
}
