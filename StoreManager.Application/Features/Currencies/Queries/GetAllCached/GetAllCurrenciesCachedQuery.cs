using StoreManager.Application.Interfaces.CacheRepositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Currencies.Queries.GetAllCached
{
    public class GetAllCurrenciesCachedQuery : IRequest<Result<List<GetAllCurrenciesCachedResponse>>>
    {
        public GetAllCurrenciesCachedQuery()
        {
        }
    }

    public class GetAllCurrenciesCachedQueryHandler : IRequestHandler<GetAllCurrenciesCachedQuery, Result<List<GetAllCurrenciesCachedResponse>>>
    {
        private readonly ICurrencyCacheRepository _productCache;
        private readonly IMapper _mapper;

        public GetAllCurrenciesCachedQueryHandler(ICurrencyCacheRepository productCache, IMapper mapper)
        {
            _productCache = productCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCurrenciesCachedResponse>>> Handle(GetAllCurrenciesCachedQuery request, CancellationToken cancellationToken)
        {
            var currencyList = await _productCache.GetCachedListAsync();
            var mappedCurrencies = _mapper.Map<List<GetAllCurrenciesCachedResponse>>(currencyList);
            return Result<List<GetAllCurrenciesCachedResponse>>.Success(mappedCurrencies);
        }
    }
}