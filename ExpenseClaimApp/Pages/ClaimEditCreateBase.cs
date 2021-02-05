using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
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
        public IMapper Mapper { get; set; }

        [Inject]
        public IClaimService ClaimService { get; set; }
        private Claim Claim { get; set; } = new Claim();
        public ClaimEditModel ClaimEditModel { get; set; } = new ClaimEditModel();
        public string RequesterComments { get; set; } = string.Empty;
        public string ApproverComments { get; set; } = string.Empty;
        public string FinanceComments { get; set; } = string.Empty;

        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ICurrencyService CurrencyService { get; set; }

        public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        public string CategoryId { get; set; }
        public List<GetAllCurrenciesResponse> Currencies { get; set; } = new List<GetAllCurrenciesResponse>();
        public string CurrencyId { get; set; }

        [Inject]
        public ILineItemService LineItemService { get; set; }
        protected List<LineItemEditModel> LineItemEditModels { get; set; } = new List<LineItemEditModel>();
        protected LineItemEditModel LineItemEditModel { get; set; } = new LineItemEditModel();
        protected LineItem LineItem { get; set; } = new LineItem();

        //used to state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();           
            Currencies = (await CurrencyService.GetCurrencies()).ToList();

            //Id = Id ?? "1";
            int.TryParse(Id, out int claimId);
            if (claimId != 0)// for Edit
            {
                Claim = (await ClaimService.GetClaimById(int.Parse(Id)));
                Mapper.Map(Claim, ClaimEditModel);

                if(ClaimEditModel.RequesterComments!=null)
                    RequesterComments = ClaimEditModel.RequesterComments;
                if (ClaimEditModel.ApproverComments != null)
                    ApproverComments = ClaimEditModel.ApproverComments;
                if (ClaimEditModel.FinanceComments != null)
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
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(ClaimEditModel, Claim);

           
            if (Claim.Id != 0)//Edit
            {
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                NavigationManager.NavigateTo("/", true);
            }
            else//Create
            {
                Claim.RequesterComments = RequesterComments;
                Claim.ApproverComments = ApproverComments;
                Claim.FinanceComments = FinanceComments;

                var result = await ClaimService.CreateClaim(Claim);
               
                if (result.Id != 0) 
                {
                    LineItem ResultItem = null;
                    foreach(var Item in LineItemEditModels)
                    {
                        Mapper.Map(Item, LineItem);
                        LineItem.ClaimId = result.Id;
                        ResultItem = await LineItemService.CreateLineItem(LineItem);
                    }                    
                    NavigationManager.NavigateTo($"/detail/{result.Id}", true);
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong Creating the new employee. Please try again.";
                    Saved = false;
                }
                
            }
        }

        protected async Task AddList_Click()
        {
            LineItemEditModel = new LineItemEditModel();
            LineItemEditModel.Date = DateTime.Now;
            LineItemEditModels.Add(LineItemEditModel);
            //NavigationManager.NavigateTo("/edit", true);
            //CreateEditMode = true;
            //CategoryEditModel = new CategoryEditModel();
        }
        protected async Task BackToList()
        {
            NavigationManager.NavigateTo("/list", true);
        }
        protected async Task Delete_Lineitem(LineItemEditModel item)
        {
            LineItemEditModels.Remove(item);
            //NavigationManager.NavigateTo("/list", true);
            
        }
    }
}
