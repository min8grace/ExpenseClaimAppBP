using StoreManager.Application.Features.Products.Commands.Create;
using StoreManager.Application.Features.Products.Queries.GetAllCached;
using StoreManager.Application.Features.Products.Queries.GetAllPaged;
using StoreManager.Application.Features.Products.Queries.GetById;
using StoreManager.Domain.Entities.Catalog;
using AutoMapper;

namespace StoreManager.Application.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, Product>().ReverseMap();
            CreateMap<GetProductByIdResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsCachedResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsResponse, Product>().ReverseMap();
        }
    }
}