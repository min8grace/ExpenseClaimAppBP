using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.Inspection
{
    public class LineItemInsBase : ComponentBase
    {

        [Inject]
        public ILineItemService LineItemService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ICurrencyService CurrencyService { get; set; }

        public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        public string CategoryId { get; set; }

        public List<GetAllCurrenciesResponse> Currencies { get; set; } = new List<GetAllCurrenciesResponse>();
        public string CurrencyId { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<GetAllLineItemsResponse> LineItems { get; set; } = new List<GetAllLineItemsResponse>();
        public int Id { get; set; }
        public StoreManager.Domain.Entities.Expense.LineItem LineItem { get; set; } = new StoreManager.Domain.Entities.Expense.LineItem();
        public LineItemEditModel LineItemEditModel { get; set; } = new LineItemEditModel();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            LineItems = (await LineItemService.GetLineItems()).ToList();

            Categories = (await CategoryService.GetCategories()).ToList();
            CategoryId = LineItem.CategoryId.ToString();
            Currencies = (await CurrencyService.GetCurrencies()).ToList();
            CurrencyId = LineItem.CurrencyId.ToString();

        }
        protected async Task Select_Click(int InputId, int b)
        {
            LineItem = await LineItemService.GetLineItemById(InputId);
            Mapper.Map(LineItem, LineItemEditModel);
        }
        protected async Task Delete_Click(int InputId)
        {
            await LineItemService.DeleteLineItem(InputId);
            NavigationManager.NavigateTo("/ins/LineItem", true);
        }
        protected async Task Create_Click()
        {
            Mapper.Map(LineItemEditModel, LineItem);

            StoreManager.Domain.Entities.Expense.LineItem result = null;
            result = await LineItemService.CreateLineItem(LineItem);
            if (result != null)
            {
                StatusClass = "alert-danger";
                Message = "Something went wrong Creating the new employee. Please try again.";
                Saved = false;
            }
            NavigationManager.NavigateTo("/ins/LineItem", true);
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(LineItemEditModel, LineItem);

            StoreManager.Domain.Entities.Expense.LineItem result = null;
            if (LineItem.Id != 0)
            {
                await LineItemService.UpdateLineItem(LineItem);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo("/ins/LineItem", true);

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
