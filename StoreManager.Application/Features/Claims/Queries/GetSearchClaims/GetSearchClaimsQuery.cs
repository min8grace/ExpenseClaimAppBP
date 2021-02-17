using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
using System;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;

namespace StoreManager.Application.Features.Claims.Queries.GetSearchClaims
{
    public class GetSearchClaimsQuery : IRequest<Result<List<GetAllClaimsResponse>>>
    {

        public string searchStr { get; set; }

        //public GetSearchClaimsQuery(string searchStr)
        //{
        //    this.searchStr = searchStr;
        //}
    }

    public class GetSearchClaimsQueryHandler : IRequestHandler<GetSearchClaimsQuery, Result<List<GetAllClaimsResponse>>>
    {
        private readonly IClaimRepository _claim;
        private readonly IMapper _mapper;

        public GetSearchClaimsQueryHandler(IClaimRepository claim, IMapper mapper)
        {
            _claim = claim;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClaimsResponse>>> Handle(GetSearchClaimsQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _claim.GetSearchClaimsAsync(request.searchStr);
            var mappedClaims = _mapper.Map<List<GetAllClaimsResponse>>(claimList);
            return Result<List<GetAllClaimsResponse>>.Success(mappedClaims);

        }
    }
}