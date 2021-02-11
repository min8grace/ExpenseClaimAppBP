using StoreManager.Application.Features.Products.Commands.Create;
using StoreManager.Application.Features.Products.Commands.Update;
using StoreManager.Application.Features.Products.Queries.GetAllCached;
using StoreManager.Application.Features.Products.Queries.GetById;
using StoreManager.Web.Areas.Catalog.Models;
using AutoMapper;

namespace StoreManager.Web.Areas.Catalog.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<GetAllProductsCachedResponse, ProductViewModel>().ReverseMap();
            CreateMap<GetProductByIdResponse, ProductViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, ProductViewModel>().ReverseMap();
            CreateMap<UpdateProductCommand, ProductViewModel>().ReverseMap();
        }
    }
}