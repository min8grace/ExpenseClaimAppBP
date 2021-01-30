using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesdQuery : IRequest<Result<List<GetAllCategoriesResponse>>>
    {
        public GetAllCategoriesdQuery()
        {
        }
    }

    public class GetAllCategoriesdQueryHandler : IRequestHandler<GetAllCategoriesdQuery, Result<List<GetAllCategoriesResponse>>>
    {
        private readonly ICategoryRepository _product;
        private readonly IMapper _mapper;

        public GetAllCategoriesdQueryHandler(ICategoryRepository product, IMapper mapper)
        {
            _product = product;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCategoriesResponse>>> Handle(GetAllCategoriesdQuery request, CancellationToken cancellationToken)
        {
            var categoryList = await _product.GetListAsync();
            var mappedCategories = _mapper.Map<List<GetAllCategoriesResponse>>(categoryList);
            return Result<List<GetAllCategoriesResponse>>.Success(mappedCategories);
        }
    }
}