using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Claims.Queries.GetById
{
    public class GetClaimByIdQuery : IRequest<Result<GetClaimByIdResponse>>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, Result<GetClaimByIdResponse>>
        {
            private readonly IClaimRepository _claim;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(IClaimRepository product, IMapper mapper)
            {
                _claim = product;
                _mapper = mapper;
            }

            public async Task<Result<GetClaimByIdResponse>> Handle(GetClaimByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _claim.GetByIdAsync(query.Id);
                var mappedProduct = _mapper.Map<GetClaimByIdResponse>(product);
                return Result<GetClaimByIdResponse>.Success(mappedProduct);
            }
        }
    }
}