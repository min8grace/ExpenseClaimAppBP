using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected IList<string> imageDataUrls = new List<string>();
        protected ImageConverter _imageConverter;// = new ImageConverter();
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


            if (LineItem.Receipt.Length > 0 ) {
                var format = "image/png";
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(LineItem.Receipt)}";
                imageDataUrls.Add(imageDataUrl);
            }

        }

        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var maxAllowedFiles = 1;
            var format = "image/png";
            if (e.GetMultipleFiles(maxAllowedFiles).Count > maxAllowedFiles)
            { Message = "max Allowed Files are 5"; return; }
            foreach (var imageFile in e.GetMultipleFiles(maxAllowedFiles))
            {
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 50, 50);

                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                if (imageDataUrl != null)
                {
                    imageDataUrls.Clear();
                    imageDataUrls.Add(imageDataUrl);
                }
                
                buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                LineItemEditModel.Receipt = buffer;
                //var image = resizedImageFile.OptimizeImageSize(700, 700);
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

        protected void Delete_Img_Click(string imageDataUrl)
        {
            imageDataUrls.Remove(imageDataUrl);
            LineItemEditModel.Receipt = null;
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(LineItemEditModel, LineItem);
            LineItem result = null;

            if (LineItem.Id != 0)//Edit
            {
                LineItem.Description = Description;
                await LineItemService.UpdateLineItem(LineItem);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo($"/edit/{LineItem.ClaimId}", true);
            }
            else//Create
            {
                result = await LineItemService.CreateLineItem(LineItem);
                if (result == null)
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong Creating the new employee. Please try again.";
                    Saved = false;
                }
                else NavigationManager.NavigateTo($"/detail/{LineItem.ClaimId}", true);
            }
        }

        protected async Task BackToList()
        {
            NavigationManager.NavigateTo($"/edit/{LineItem.ClaimId}", true);
        }
    }
}
