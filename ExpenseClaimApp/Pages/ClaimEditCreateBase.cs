using AutoMapper;
using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

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

        protected List<LineItemImageModel> LineItemImageModels = new List<LineItemImageModel>();
        protected IList<string> imageDataUrls = new List<string>();
        protected ImageConverter _imageConverter;// = new ImageConverter();

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

                if (ClaimEditModel.RequesterComments != null)
                    RequesterComments = ClaimEditModel.RequesterComments;
                if (ClaimEditModel.ApproverComments != null)
                    ApproverComments = ClaimEditModel.ApproverComments;
                if (ClaimEditModel.FinanceComments != null)
                    FinanceComments = ClaimEditModel.FinanceComments;
                if (Claim.LineItems.Count() > 0)
                {
                    foreach (var LineItem in Claim.LineItems)
                    {
                        if (LineItem.Receipt != null)
                        {
                            imageDataUrls.Clear();
                            var format = "image/png";
                            var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(LineItem.Receipt)}";
                            imageDataUrls.Add(imageDataUrl);
                            LineItemImageModel Lim = new LineItemImageModel { Id = LineItem.Id, ImageDataUrls = imageDataUrls.ToList() };
                            LineItemImageModels.Add(Lim);
                        }
                    }
                }
            }
            else // for Create
            {
                ClaimEditModel = new ClaimEditModel
                {
                    Requester = Name,
                    SubmitDate = DateTime.Now,
                    Status = (Status)Enum.Parse(typeof(Status), "Requested")
                };
                if (LineItemEditModels.Count() > 0)
                {
                    foreach (var lineItemEditModel in LineItemEditModels)
                    {
                        if (lineItemEditModel.Receipt != null)
                        {
                            imageDataUrls.Clear();
                            var format = "image/png";
                            var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(lineItemEditModel.Receipt)}";
                            imageDataUrls.Add(imageDataUrl);
                            LineItemImageModel Lim = new LineItemImageModel { Id = lineItemEditModel.Id, ImageDataUrls = imageDataUrls.ToList() };
                            LineItemImageModels.Add(Lim);
                        }
                    }
                }
            }
        }

        protected async Task OnInputFileChange(InputFileChangeEventArgs e, LineItemEditModel Liem)
        {
            var maxAllowedFiles = 1;
            var format = "image/png";
            if (e.GetMultipleFiles(maxAllowedFiles).Count > maxAllowedFiles)
            { Message = "max Allowed Files are 5"; return; }
            foreach (var imageFile in e.GetMultipleFiles(maxAllowedFiles))
            {
                //var resizedImageFile = await imageFile.RequestImageFileAsync(format, 100, 100);
                //var buffer = new byte[resizedImageFile.Size];
                //await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                if (imageDataUrl != null)
                {
                    imageDataUrls.Clear();
                    imageDataUrls.Add(imageDataUrl);
                }

                buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);

                if (Claim.Id != 0)// for Edit
                {
                    LineItemEditModel.Receipt = buffer;
                }
                else
                {
                    LineItemEditModels[LineItemEditModels.IndexOf(Liem)].Receipt = buffer;
                    LineItemImageModel Lim = new LineItemImageModel { Id = Liem.Id, ImageDataUrls = imageDataUrls.ToList() };
                    LineItemImageModels.Add(Lim);
                }
                    
                //var image = resizedImageFile.OptimizeImageSize(700, 700);
            }
        }

        protected void Delete_Img_Click(string imageDataUrl)
        {
            imageDataUrls.Remove(imageDataUrl);
            LineItemEditModel.Receipt = null;
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
            NavigationManager.NavigateTo("/list", true);
        }
        protected async Task Delete_Lineitem(LineItemEditModel item)
        {
            LineItemImageModels.Remove(LineItemImageModels.Where(x => x.Id == LineItemEditModels.IndexOf(item)).FirstOrDefault());
            LineItemEditModels.Remove(item);
            ClaimEditModel.TotalAmount = LineItemEditModels.Select(x => x.USDAmount).Sum(x => x);
            //NavigationManager.NavigateTo("/list", true);
        }

        protected async Task Delete_Click(int SelectedId, int SeletedClaimId)
        {
            await LineItemService.DeleteLineItem(SelectedId);
            NavigationManager.NavigateTo($"/edit/{SeletedClaimId}", true); 
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
