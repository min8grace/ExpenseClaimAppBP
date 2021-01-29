using StoreManager.Application.Interfaces.CacheRepositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Currencies.Queries.GetById
{
    public class GetCurrencyByIdQuery : IRequest<Result<GetCurrencyByIdResponse>>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, Result<GetCurrencyByIdResponse>>
        {
            private readonly ICurrencyCacheRepository _currencyCache;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(ICurrencyCacheRepository productCache, IMapper mapper)
            {
                _currencyCache = productCache;
                _mapper = mapper;
            }

            public async Task<Result<GetCurrencyByIdResponse>> Handle(GetCurrencyByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _currencyCache.GetByIdAsync(query.Id);
                var mappedProduct = _mapper.Map<GetCurrencyByIdResponse>(product);
                return Result<GetCurrencyByIdResponse>.Success(mappedProduct);
            }
        }
    }
}