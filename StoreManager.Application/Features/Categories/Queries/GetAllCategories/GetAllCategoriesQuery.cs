using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<Result<List<GetAllCategoriesResponse>>>
    {
        public GetAllCategoriesQuery()
        {
        }
    }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>>
    {
        private readonly ICategoryRepository _product;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(ICategoryRepository product, IMapper mapper)
        {
            _product = product;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categoryList = await _product.GetListAsync();
            var mappedCategories = _mapper.Map<List<GetAllCategoriesResponse>>(categoryList);
            return Result<List<GetAllCategoriesResponse>>.Success(mappedCategories);
        }
    }
}