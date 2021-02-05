using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.Inspection
{
    public class CategoryInsBase : ComponentBase
    {

        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        public List<GetAllCategoriesResponse> Categories { get; set; } = new List<GetAllCategoriesResponse>();
        public int Id { get; set; }
        public StoreManager.Domain.Entities.Expense.Category Category { get; set; } = new StoreManager.Domain.Entities.Expense.Category();
        public CategoryEditModel CategoryEditModel { get; set; } = new CategoryEditModel();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected bool CreateEditMode { get; set; } = false;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CreateEditMode = false;
            CategoryEditModel = new CategoryEditModel();
            Categories = (await CategoryService.GetCategories()).ToList();
        }
        protected async Task Edit_Click(int InputId, int b)
        {
            CreateEditMode = true;
            Category = await CategoryService.GetCategoryById(InputId);
            Mapper.Map(Category, CategoryEditModel);
        }
        protected async Task Delete_Click(int InputId)
        {
            await CategoryService.DeleteCategory(InputId);
            NavigationManager.NavigateTo("/ins/Category", true);
        }
        protected async Task Create_Click()
        {
            CreateEditMode = true;
            CategoryEditModel = new CategoryEditModel();
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(CategoryEditModel, Category);

            if (Category.Id != 0) //Edit
            {
                await CategoryService.UpdateCategory(Category);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo("/ins/Category", true);

            }
            else //Create
            {
                StoreManager.Domain.Entities.Expense.Category result = null;
                result = await CategoryService.CreateCategory(Category);
                if (result != null)
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong Creating the new employee. Please try again.";
                    Saved = false;
                }
                NavigationManager.NavigateTo("/ins/Category", true);
            }
        }
    }
}
