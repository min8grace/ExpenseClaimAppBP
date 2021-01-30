using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies
{
    public class GetAllCurrenciesQuery : IRequest<Result<List<GetAllCurrenciesResponse>>>
    {
        public GetAllCurrenciesQuery()
        {
        }
    }

    public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, Result<List<GetAllCurrenciesResponse>>>
    {
        private readonly ICurrencyRepository _product;
        private readonly IMapper _mapper;

        public GetAllCurrenciesQueryHandler(ICurrencyRepository product, IMapper mapper)
        {
            _product = product;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCurrenciesResponse>>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencyList = await _product.GetListAsync();
            var mappedCurrencies = _mapper.Map<List<GetAllCurrenciesResponse>>(currencyList);
            return Result<List<GetAllCurrenciesResponse>>.Success(mappedCurrencies);
        }
    }
}