using StoreManager.Application.Features.Brands.Commands.Create;
using StoreManager.Application.Features.Brands.Commands.Update;
using StoreManager.Application.Features.Brands.Queries.GetAllCached;
using StoreManager.Application.Features.Brands.Queries.GetById;
using StoreManager.Web.Areas.Catalog.Models;
using AutoMapper;

namespace StoreManager.Web.Areas.Catalog.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<GetAllBrandsCachedResponse, BrandViewModel>().ReverseMap();
            CreateMap<GetBrandByIdResponse, BrandViewModel>().ReverseMap();
            CreateMap<CreateBrandCommand, BrandViewModel>().ReverseMap();
            CreateMap<UpdateBrandCommand, BrandViewModel>().ReverseMap();
        }
    }
}