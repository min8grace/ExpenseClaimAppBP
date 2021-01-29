using StoreManager.Application.Interfaces.CacheRepositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.LineItems.Queries.GetById
{
    public class GetLineItemByIdQuery : IRequest<Result<GetLineItemByIdResponse>>
    {
        public int Id { get; set; }

        public class GetLineItemByIdQueryHandler : IRequestHandler<GetLineItemByIdQuery, Result<GetLineItemByIdResponse>>
        {
            private readonly ILineItemCacheRepository _lineItemCache;
            private readonly IMapper _mapper;

            public GetLineItemByIdQueryHandler(ILineItemCacheRepository lineItemCache, IMapper mapper)
            {
                _lineItemCache = lineItemCache;
                _mapper = mapper;
            }

            public async Task<Result<GetLineItemByIdResponse>> Handle(GetLineItemByIdQuery query, CancellationToken cancellationToken)
            {
                var lineItem = await _lineItemCache.GetByIdAsync(query.Id);
                var mappedLineItem = _mapper.Map<GetLineItemByIdResponse>(lineItem);
                return Result<GetLineItemByIdResponse>.Success(mappedLineItem);
            }
        }
    }
}