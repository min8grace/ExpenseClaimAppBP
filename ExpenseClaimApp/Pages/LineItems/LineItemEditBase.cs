using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.LineItems
{
    public class LineItemEditBase : ComponentBase
    {
        [Inject]
        public ILineItemService LineItemService { get; set; }
        private LineItem LineItem { get; set; } = new LineItem();
        public LineItemEditModel LineItemEditModel { get; set; } = new LineItemEditModel();

        [Inject]
        public ICategoryService CategoryService { get; set; }

        public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        public string CategoryId { get; set; }

        [Inject]
        public ICurrencyService CurrencyService { get; set; }

        public List<GetAllCurrenciesResponse> Currencies { get; set; } = new List<GetAllCurrenciesResponse>();
        public string CurrencyId { get; set; }


        public string Description { get; set; } = string.Empty;

        [Inject]
        public IMapper Mapper { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Id = Id ?? "1";
            //int.TryParse(Id, out int Id);
            if (Id != null) // for Edit
            {
                LineItem = (await LineItemService.GetLineItemById(int.Parse(Id)));
                Mapper.Map(LineItem, LineItemEditModel);
                Description = LineItemEditModel.Description;
            }
            else // for Create
            {
                //PageHeader = "Create LineItem";
                LineItem = new LineItem
                {
                    //LineItems Id = 1,
                    Date = DateTime.Now,
                    Description = " ",
                    //Status = (Status)Enum.Parse(typeof(Status), "Requested")
                };
            }

            Categories = (await CategoryService.GetCategories()).ToList();
            CategoryId = LineItem.CategoryId.ToString();
            Currencies = (await CurrencyService.GetCurrencies()).ToList();
            CurrencyId = LineItem.CurrencyId.ToString();
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(LineItemEditModel, LineItem);

            LineItem result = null;
            if (LineItem.Id != 0)
            {
                NavigationManager.NavigateTo($"/detail/{LineItem.ClaimId}", true);
            }
            else
            {
                result = await LineItemService.CreateLineItem(LineItem);
                if (result != null) NavigationManager.NavigateTo($"/detail/{LineItem.ClaimId}", true);
            }


        }
    }
}
