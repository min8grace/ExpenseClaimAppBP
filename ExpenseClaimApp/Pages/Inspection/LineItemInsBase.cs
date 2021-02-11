using AutoMapper;
using ExpenseClaimApp.Extensions;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        protected bool CreateEditMode { get; set; } = false;

        [Inject]
        public NavigationManager NavigationManager { get; set; }




        protected IList<string> imageDataUrls = new List<string>();
        protected ImageConverter _imageConverter;// = new ImageConverter();
        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {

            var maxAllowedFiles = 1;
            var format = "image/png";
            if (e.GetMultipleFiles(maxAllowedFiles).Count > maxAllowedFiles)
              { Message = "max Allowed Files are 5"; return; }
            foreach (var imageFile in e.GetMultipleFiles(maxAllowedFiles))
            {
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 100, 100);

                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                if(imageDataUrl != null)
                {
                    imageDataUrls.Clear();
                    imageDataUrls.Add(imageDataUrl);
                }                    
                LineItemEditModel.Receipt = buffer;
                //var image = resizedImageFile.OptimizeImageSize(700, 700);
            }
        }


        protected override async Task OnInitializedAsync()
        {
            CreateEditMode = false;
            LineItemEditModel = new LineItemEditModel();

            LineItems = (await LineItemService.GetLineItems()).ToList();
            Categories = (await CategoryService.GetCategories()).ToList();
            CategoryId = LineItem.CategoryId.ToString();
            Currencies = (await CurrencyService.GetCurrencies()).ToList();
            CurrencyId = LineItem.CurrencyId.ToString();
        }
        
        protected  void Delete_Img_Click(string imageDataUrl)
        {
            imageDataUrls.Remove(imageDataUrl);
            LineItemEditModel.Receipt = null;
        }
        protected async Task Edit_Click(int InputId, int b)
        {
            CreateEditMode = true;
            LineItem = await LineItemService.GetLineItemById(InputId);
            Mapper.Map(LineItem, LineItemEditModel);
            Description = LineItem.Description;
            // image

            if (LineItem.Receipt.Length > 0)
            {
                var format = "image/png";
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(LineItem.Receipt)}";
                imageDataUrls.Add(imageDataUrl);
            }

        }
        protected async Task Delete_Click(int InputId)
        {
            await LineItemService.DeleteLineItem(InputId);
            NavigationManager.NavigateTo("/ins/LineItem", true);
        }
        protected async Task Create_Click()
        {
            CreateEditMode = true;
            LineItemEditModel = new LineItemEditModel();
            imageDataUrls = new List<string>();
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(LineItemEditModel, LineItem);
            
            if (LineItem.Id != 0)
            {
                LineItem.Description = Description;
                await LineItemService.UpdateLineItem(LineItem);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo("/ins/LineItem", true);
            }
            else//Create
            {
                StoreManager.Domain.Entities.Expense.LineItem result = null;
                result = await LineItemService.CreateLineItem(LineItem);
                if (result == null)
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong Creating the new employee. Please try again.";
                    Saved = false;
                }else NavigationManager.NavigateTo("/ins/LineItem", true);
            }
        }
    }
}
