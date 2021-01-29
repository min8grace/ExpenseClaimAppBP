using AutoMapper;
using StoreManager.Application.Features.LineItems.Commands.Create;
using StoreManager.Application.Features.LineItems.Queries.GetAllCached;
using StoreManager.Application.Features.LineItems.Queries.GetAllPaged;
using StoreManager.Application.Features.LineItems.Queries.GetById;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Mappings
{
    internal class LineItemProfile : Profile
    {
        public LineItemProfile()
        {
            CreateMap<CreateLineItemCommand, LineItem>().ReverseMap();
            CreateMap<GetLineItemByIdResponse, LineItem>().ReverseMap();
            CreateMap<GetAllLineItemsCachedResponse, LineItem>().ReverseMap();
            CreateMap<GetAllLineItemsResponse, LineItem>().ReverseMap();
        }
    }
}
