using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.LineItems.Queries.GetAllLineItems
{
    public class GetAllPagedLineItemsQuery : IRequest<Result<List<GetAllLineItemsResponse>>>
    {
        public GetAllPagedLineItemsQuery()
        {
        }
    }

    public class GetAllPagedLineItemsQueryHandler : IRequestHandler<GetAllPagedLineItemsQuery, Result<List<GetAllLineItemsResponse>>>
    {
        private readonly ILineItemRepository _lineItem;
        private readonly IMapper _mapper;

        public GetAllPagedLineItemsQueryHandler(ILineItemRepository lineItem, IMapper mapper)
        {
            _lineItem = lineItem;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllLineItemsResponse>>> Handle(GetAllPagedLineItemsQuery request, CancellationToken cancellationToken)
        {
            var lineItemList = await _lineItem.GetListAsync();
            var mappedLineItems = _mapper.Map<List<GetAllLineItemsResponse>>(lineItemList);
            return Result<List<GetAllLineItemsResponse>>.Success(mappedLineItems);
        }
    }
}