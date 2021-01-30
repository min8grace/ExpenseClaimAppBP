using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.LineItems.Queries.GetAllLineItems
{
    public class GetAllLineItemsQuery : IRequest<Result<List<GetAllLineItemsResponse>>>
    {
        public GetAllLineItemsQuery()
        {
        }
    }

    public class GetAllLineItemsQueryHandler : IRequestHandler<GetAllLineItemsQuery, Result<List<GetAllLineItemsResponse>>>
    {
        private readonly ILineItemRepository _lineItem;
        private readonly IMapper _mapper;

        public GetAllLineItemsQueryHandler(ILineItemRepository lineItem, IMapper mapper)
        {
            _lineItem = lineItem;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllLineItemsResponse>>> Handle(GetAllLineItemsQuery request, CancellationToken cancellationToken)
        {
            var lineItemList = await _lineItem.GetListAsync();
            var mappedLineItems = _mapper.Map<List<GetAllLineItemsResponse>>(lineItemList);
            return Result<List<GetAllLineItemsResponse>>.Success(mappedLineItems);
        }
    }
}