
using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Features.Claims.Queries.GetById
{
    public class GetClaimByIdQueryToClaim : IRequest<Result<Claim>>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetClaimByIdQueryToClaim, Result<Claim>>
        {
            private readonly IClaimRepository _claim;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(IClaimRepository claim, IMapper mapper)
            {
                _claim = claim;
                _mapper = mapper;
            }

            public async Task<Result<Claim>> Handle(GetClaimByIdQueryToClaim query, CancellationToken cancellationToken)
            {
                var claim = await _claim.GetByIdAsync(query.Id);
                var mappedClaim = _mapper.Map<GetClaimByIdResponse>(claim);
                return Result<Claim>.Success(claim);
            }
        }
    }
}