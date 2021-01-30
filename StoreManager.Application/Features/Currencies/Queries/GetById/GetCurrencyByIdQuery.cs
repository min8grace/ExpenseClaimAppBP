using StoreManager.Application.Interfaces.Repositories;
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
            private readonly ICurrencyRepository _currency;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(ICurrencyRepository product, IMapper mapper)
            {
                _currency = product;
                _mapper = mapper;
            }

            public async Task<Result<GetCurrencyByIdResponse>> Handle(GetCurrencyByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _currency.GetByIdAsync(query.Id);
                var mappedProduct = _mapper.Map<GetCurrencyByIdResponse>(product);
                return Result<GetCurrencyByIdResponse>.Success(mappedProduct);
            }
        }
    }
}