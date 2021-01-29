using StoreManager.Application.Interfaces.CacheRepositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
namespace StoreManager.Application.Features.Claims.Queries.GetAllCached
{
    public class GetAllClaimsCachedQuery : IRequest<Result<List<GetAllClaimsCachedResponse>>>
    {
        public GetAllClaimsCachedQuery()
        {
        }
    }

    public class GetAllClaimsCachedQueryHandler : IRequestHandler<GetAllClaimsCachedQuery, Result<List<GetAllClaimsCachedResponse>>>
    {
        private readonly IClaimCacheRepository _productCache;
        private readonly IMapper _mapper;

        public GetAllClaimsCachedQueryHandler(IClaimCacheRepository productCache, IMapper mapper)
        {
            _productCache = productCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClaimsCachedResponse>>> Handle(GetAllClaimsCachedQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _productCache.GetCachedListAsync();
            var mappedClaims = _mapper.Map<List<GetAllClaimsCachedResponse>>(claimList);
            return Result<List<GetAllClaimsCachedResponse>>.Success(mappedClaims);
        }
    }
}