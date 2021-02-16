using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
namespace ExpenseClaimApp.Pages.LineItems
{
    public class LineItemDetailBase : ComponentBase
    {
        [Inject]
        public ILineItemService LineItemService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ICurrencyService CurrencyService { get; set; }
        public LineItem LineItem { get; set; }
        public Category Category { get; set; }
        public Currency Currency { get; set; }

        public IEnumerable<LineItem> LineItems { get; set; }
        protected IList<string> imageDataUrls = new List<string>();
        protected ImageConverter _imageConverter;// = new ImageConverter();

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Id = Id ?? "3";
            LineItem = await LineItemService.GetLineItemById(int.Parse(Id));
            Category = await CategoryService.GetCategoryById(LineItem.CategoryId);
            Currency = await CurrencyService.GetCurrencyById(LineItem.CurrencyId);

            if (LineItem.Receipt.Length > 0)
            {
                var format = "image/png";

                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(LineItem.Receipt)}";
                imageDataUrls.Add(imageDataUrl);
            }
        }

        protected string selectedImage;
        protected void SelectImage(string imageDataUrl)
        {
            ShowDialog = true;
            selectedImage = imageDataUrl;
        }
        public bool ShowDialog { get; set; }
        protected void CloseModal()
        {
            ShowDialog = false;
            StateHasChanged();
        }


        protected async Task BackToList()
        {
            NavigationManager.NavigateTo($"/detail/{LineItem.ClaimId}", true);
        }


    }
}
