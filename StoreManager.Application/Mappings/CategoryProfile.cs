using AutoMapper;
using StoreManager.Application.Features.Categories.Queries.GetAllCached;
using StoreManager.Application.Features.Categories.Queries.GetById;
using StoreManager.Application.Features.Categorys.Commands.Create;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Mappings
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryCommand, Category>().ReverseMap();
            CreateMap<GetCategoryByIdResponse, Category>().ReverseMap();
            CreateMap<GetAllCategoriesCachedResponse, Category>().ReverseMap();
        }
    }
}
