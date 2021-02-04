using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages
{

    public class ClaimEditCreateBase : ComponentBase
    {
        [Inject]
        public IClaimService ClaimService { get; set; }
        private Claim Claim { get; set; } = new Claim();
        public ClaimEditModel ClaimEditModel { get; set; } = new ClaimEditModel();

        [Inject]
        public ICategoryService CategoryService { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        public string CategoryId { get; set; }

        public string RequesterComments { get; set; } = string.Empty;
        public string ApproverComments { get; set; } = string.Empty;
        public string FinanceComments { get; set; } = string.Empty;


        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Id = Id ?? "1";
            int.TryParse(Id, out int claimId);
            if (claimId != 0)// for Edit
            {
                Claim = (await ClaimService.GetClaimById(int.Parse(Id)));
                Mapper.Map(Claim, ClaimEditModel);


                RequesterComments = ClaimEditModel.RequesterComments;
                ApproverComments = ClaimEditModel.ApproverComments;
                FinanceComments = ClaimEditModel.FinanceComments;
            }
            else // for Create
            {
                ClaimEditModel = new ClaimEditModel
                {
                    SubmitDate = DateTime.Now,
                    Status = (Status)Enum.Parse(typeof(Status), "Requested")
                };
            }

            Categories = (await CategoryService.GetCategories()).ToList();
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(ClaimEditModel, Claim);

            Claim result = null;
            if (Claim.Id != 0)
            {
                NavigationManager.NavigateTo("/", true);
            }
            else
            {
                result = await ClaimService.CreateClaim(Claim);
                if (result != null) NavigationManager.NavigateTo("/", true);
            }

        }
    }
}
