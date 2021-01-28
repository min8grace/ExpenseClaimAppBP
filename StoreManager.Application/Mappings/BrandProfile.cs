using StoreManager.Application.Features.Brands.Commands.Create;
using StoreManager.Application.Features.Brands.Queries.GetAllCached;
using StoreManager.Application.Features.Brands.Queries.GetById;
using StoreManager.Domain.Entities.Catalog;
using AutoMapper;

namespace StoreManager.Application.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsCachedResponse, Brand>().ReverseMap();
        }
    }
}