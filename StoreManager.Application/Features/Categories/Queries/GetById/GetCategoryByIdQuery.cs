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

        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryByIdResponse>>
        {
            private readonly ICategoryRepository _category;
            private readonly IMapper _mapper;

            public GetCategoryByIdQueryHandler(ICategoryRepository category, IMapper mapper)
            {
                _category = category;
                _mapper = mapper;
            }

            public async Task<Result<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                var category = await _category.GetByIdAsync(query.Id);
                var mappedCategory = _mapper.Map<GetCategoryByIdResponse>(category);
                return Result<GetCategoryByIdResponse>.Success(mappedCategory);
            }
        }
    }
}