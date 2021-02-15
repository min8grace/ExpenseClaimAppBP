using AutoMapper;
using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        protected ClaimEditModel ClaimEditModel { get; set; } = new ClaimEditModel();
        protected string RequesterComments { get; set; } = "N/A";
        protected string ApproverComments { get; set; } = "N/A";
        protected string FinanceComments { get; set; } = "N/A";

        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ICurrencyService CurrencyService { get; set; }

        public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        protected string CategoryId { get; set; }
        public List<GetAllCurrenciesResponse> Currencies { get; set; } = new List<GetAllCurrenciesResponse>();
        protected string CurrencyId { get; set; }

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

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        protected string Name { get; set; }
        protected string Role { get; set; }
        protected override async Task OnInitializedAsync()
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
                    Requester = Name,
                    SubmitDate = DateTime.Now,                    
                    Status = (Status)Enum.Parse(typeof(Status), "Requested")
                };
            }          


        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(ClaimEditModel, Claim);
            Claim.RequesterComments = RequesterComments;
            Claim.ApproverComments = ApproverComments;
            Claim.FinanceComments = FinanceComments;
            Claim.SubmitDate = DateTime.Now;
            Claim.Requester = Name;//Id(email)
            Claim.Status = Status.Requested;
            if (Claim.Id != 0)//Edit-Submit
            {
                await ClaimService.UpdateClaim(Claim);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                NavigationManager.NavigateTo("/list", true);
            }
            else//Create-Submit
            {
                var result = await ClaimService.CreateClaim(Claim);
                if (result.Id != 0)
                {
                    LineItem ResultItem = null;
                    foreach (var Item in LineItemEditModels)
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

        protected async Task Save_Click()
        {
            Mapper.Map(ClaimEditModel, Claim);
            Claim.RequesterComments = RequesterComments;
            Claim.ApproverComments = ApproverComments;
            Claim.FinanceComments = FinanceComments;
            if (Claim.Id != 0)//Edit-Save
            {                
                await ClaimService.UpdateClaim(Claim);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                NavigationManager.NavigateTo("/list", true);
            }
            else//Create-Save
            {
                Claim.Status = Status.Saved;
                var result = await ClaimService.CreateClaim(Claim);
                if (result.Id != 0)
                {
                    LineItem ResultItem = null;
                    foreach (var Item in LineItemEditModels)
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
            LineItemEditModel.CurrencyId = 7;//USD
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
            ClaimEditModel.TotalAmount = LineItemEditModels.Select(x => x.USDAmount).Sum(x => x);
            //NavigationManager.NavigateTo("/list", true);

        }
        protected async Task EventAmt(int i, ChangeEventArgs e)
        {
            var value = 0;
            if (Int32.TryParse(e.Value.ToString(), out int auxn)) value = auxn;
            LineItemEditModels[i].USDAmount = value;
            ClaimEditModel.TotalAmount = LineItemEditModels.Select(x => x.USDAmount).Sum(x => x);
            //StateHasChanged();
        }      
    }
}
