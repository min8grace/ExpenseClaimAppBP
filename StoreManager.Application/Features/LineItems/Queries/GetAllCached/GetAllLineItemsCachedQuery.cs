using StoreManager.Application.Interfaces.CacheRepositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.LineItems.Queries.GetAllCached
{
    public class GetAllLineItemsCachedQuery : IRequest<Result<List<GetAllLineItemsCachedResponse>>>
    {
        public GetAllLineItemsCachedQuery()
        {
        }
    }

    public class GetAllLineItemsCachedQueryHandler : IRequestHandler<GetAllLineItemsCachedQuery, Result<List<GetAllLineItemsCachedResponse>>>
    {
        private readonly ILineItemCacheRepository _lineItemCache;
        private readonly IMapper _mapper;

        public GetAllLineItemsCachedQueryHandler(ILineItemCacheRepository lineItemCache, IMapper mapper)
        {
            _lineItemCache = lineItemCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllLineItemsCachedResponse>>> Handle(GetAllLineItemsCachedQuery request, CancellationToken cancellationToken)
        {
            var lineItemList = await _lineItemCache.GetCachedListAsync();
            var mappedLineItems = _mapper.Map<List<GetAllLineItemsCachedResponse>>(lineItemList);
            return Result<List<GetAllLineItemsCachedResponse>>.Success(mappedLineItems);
        }
    }
}