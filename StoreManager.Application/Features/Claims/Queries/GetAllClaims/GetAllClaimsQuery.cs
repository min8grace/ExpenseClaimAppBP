﻿using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
namespace StoreManager.Application.Features.Claims.Queries.GetAllClaims
{
    public class GetAllClaimsQuery : IRequest<Result<List<GetAllClaimsResponse>>>
    {
        public GetAllClaimsQuery()
        {
        }
    }

    public class GetAllClaimsQueryHandler : IRequestHandler<GetAllClaimsQuery, Result<List<GetAllClaimsResponse>>>
    {
        private readonly IClaimRepository _claim;
        private readonly IMapper _mapper;

        public GetAllClaimsQueryHandler(IClaimRepository claim, IMapper mapper)
        {
            _claim = claim;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClaimsResponse>>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _claim.GetListAsync();
            var mappedClaims = _mapper.Map<List<GetAllClaimsResponse>>(claimList);
            return Result<List<GetAllClaimsResponse>>.Success(mappedClaims);
        }
    }
}