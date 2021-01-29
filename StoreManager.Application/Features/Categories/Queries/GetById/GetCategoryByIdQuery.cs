using StoreManager.Application.Interfaces.CacheRepositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Categories.Queries.GetById
{
    public class GetCategoryByIdQuery : IRequest<Result<GetCategoryByIdResponse>>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryByIdResponse>>
        {
            private readonly ICategoryCacheRepository _categoryCache;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(ICategoryCacheRepository productCache, IMapper mapper)
            {
                _categoryCache = productCache;
                _mapper = mapper;
            }

            public async Task<Result<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _categoryCache.GetByIdAsync(query.Id);
                var mappedProduct = _mapper.Map<GetCategoryByIdResponse>(product);
                return Result<GetCategoryByIdResponse>.Success(mappedProduct);
            }
        }
    }
}