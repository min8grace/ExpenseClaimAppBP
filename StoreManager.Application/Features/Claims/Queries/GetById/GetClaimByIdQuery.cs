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

            public GetProductByIdQueryHandler(IClaimRepository claim, IMapper mapper)
            {
                _claim = claim;
                _mapper = mapper;
            }

            public async Task<Result<GetClaimByIdResponse>> Handle(GetClaimByIdQuery query, CancellationToken cancellationToken)
            {
                var claim = await _claim.GetByIdAsync(query.Id);
                var mappedClaim = _mapper.Map<GetClaimByIdResponse>(claim);
                return Result<GetClaimByIdResponse>.Success(mappedClaim);
            }
        }
    }
}