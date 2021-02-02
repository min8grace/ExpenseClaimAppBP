using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public interface ICategoryService
    {
        Task<List<GetAllCategoriesResponse>> GetCategories();

        Task<Category> GetCategoryById(int id);

        Task<Category> CreateCategory(Category newCategory);

        Task UpdateCategory(Category updatedCategory);

        Task DeleteCategory(int id);
    }
}
