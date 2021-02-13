using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.Processes
{
    public class ApproverClaimBase : ComponentBase
    {

        [Inject]
        public IClaimService ClaimService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }
        public Claim Claim { get; set; }
        public ClaimEditModel ClaimEditModel { get; set; } = new ClaimEditModel();
        public IEnumerable<Claim> Claims { get; set; }
        public enum StatusApprover
        {
            Approved = 2, Rejected = 3, Queried = 4
        }
        public StatusApprover SelectedStatus { get; set; }
        public string ApproverComments { get; set; } = string.Empty;
        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        [Parameter]
        public string Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
       
            Claim = await ClaimService.GetClaimById(int.Parse(Id));
            ApproverComments = Claim.ApproverComments;
        }

        protected async Task BackToList()
        {
            NavigationManager.NavigateTo("/list", true);
        }
        protected async Task HandleValidSubmit()
        {
                //Mapper.Map(ClaimEditModel, Claim);       
                Claim.ApproverComments = ApproverComments;
                Claim.Status = (Status)SelectedStatus;
                Claim.ApprovalDate = DateTime.Now;
                await ClaimService.UpdateClaim(Claim);

                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo("/list", true);           
        }
    }
}
