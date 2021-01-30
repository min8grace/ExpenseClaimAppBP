using AutoMapper;
using StoreManager.Application.Features.LineItems.Commands.Create;
using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using StoreManager.Application.Features.LineItems.Queries.GetAllPaged;
using StoreManager.Application.Features.LineItems.Queries.GetById;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Mappings
{
    internal class LineItemProfile : Profile
    {
        public LineItemProfile()
        {
            CreateMap<CreateLineItemCommand, LineItem>().ReverseMap();
            CreateMap<GetLineItemByIdResponse, LineItem>().ReverseMap();
            CreateMap<GetAllLineItemsResponse, LineItem>().ReverseMap();
            CreateMap<GetAllPagedLinItemsResponse, LineItem>().ReverseMap();
        }
    }
}
