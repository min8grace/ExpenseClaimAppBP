using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace ExpenseClaimApp.Pages.Inspection
{
    public class ClaimInsBase : ComponentBase
    {

        [Inject]
        public IClaimService ClaimService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        //[Inject]
        //public ICategoryService CategoryService { get; set; }
        //[Inject]
        //public ICurrencyService CurrencyService { get; set; }

        //public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        //public string CategoryId { get; set; }

        //public List<GetAllCurrenciesResponse> Currencies { get; set; } = new List<GetAllCurrenciesResponse>();
        //public string CurrencyId { get; set; }


        public string RequesterComments { get; set; } = string.Empty;
        public string ApproverComments { get; set; } = string.Empty;
        public string FinanceComments { get; set; } = string.Empty;

        public List<GetAllClaimsResponse> Claims { get; set; } = new List<GetAllClaimsResponse>();
        public Claim Claim { get; set; } = new Claim();
        public int Id { get; set; }
        
        public ClaimEditModel ClaimEditModel { get; set; } = new ClaimEditModel();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Claims = (await ClaimService.GetClaims()).ToList();

            //Categories = (await CategoryService.GetCategories()).ToList();
            //CategoryId = Claim.CategoryId.ToString();
            //Currencies = (await CurrencyService.GetCurrencies()).ToList();
            //CurrencyId = Claim.CurrencyId.ToString();

        }
        protected async Task Select_Click(int InputId, int b)
        {
            Claim = await ClaimService.GetClaimById(InputId);
            Mapper.Map(Claim, ClaimEditModel);
            RequesterComments = Claim.RequesterComments;
            ApproverComments = Claim.ApproverComments;
            FinanceComments = Claim.FinanceComments;
        }
        protected async Task Delete_Click(int InputId)
        {
            await ClaimService.DeleteClaim(InputId);
            NavigationManager.NavigateTo("/ins/Claim", true);
        }
        protected async Task Create_Click()
        {
            Mapper.Map(ClaimEditModel, Claim);

            StoreManager.Domain.Entities.Expense.Claim result = null;
            result = await ClaimService.CreateClaim(Claim);
            if (result != null)
            {
                StatusClass = "alert-danger";
                Message = "Something went wrong Creating the new employee. Please try again.";
                Saved = false;
            }
            NavigationManager.NavigateTo("/ins/Claim", true);
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(ClaimEditModel, Claim);

            StoreManager.Domain.Entities.Expense.Claim result = null;
            if (Claim.Id != 0)
            {
                Claim.RequesterComments = RequesterComments;
                Claim.ApproverComments = ApproverComments;
                Claim.FinanceComments = FinanceComments;
                await ClaimService.UpdateClaim(Claim);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo("/ins/Claim", true);

            }
            else
            {
                StatusClass = "alert-danger";
                Message = "Something went wrong updating the new employee. Please try again.";
                Saved = false;
            }

        }
    }
}
