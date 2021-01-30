using StoreManager.Application.Interfaces.Repositories;
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
            private readonly ICategoryRepository _category;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(ICategoryRepository product, IMapper mapper)
            {
                _category = product;
                _mapper = mapper;
            }

            public async Task<Result<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _category.GetByIdAsync(query.Id);
                var mappedProduct = _mapper.Map<GetCategoryByIdResponse>(product);
                return Result<GetCategoryByIdResponse>.Success(mappedProduct);
            }
        }
    }
}