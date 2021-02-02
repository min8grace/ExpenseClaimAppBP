using AutoMapper;
using StoreManager.Application.Features.Categories.Commands.Create;
using StoreManager.Application.Features.Categories.Commands.Update;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {

            CreateMap<Category, CategoryEditModel>();
            CreateMap<CategoryEditModel, Category>();
            CreateMap<Category, UpdateCategoryCommand>();
            CreateMap<Category, CreateCategoryCommand>();
        }
    }
}
