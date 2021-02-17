using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Claims.Queries.GetSearchClaims;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public interface IClaimService
    {
        Task<List<GetAllClaimsResponse>> GetClaims();

        Task<List<GetAllClaimsResponse>> GetSearchClaims(string searchStr);

        Task<Claim> GetClaimById(int id);

        Task<Claim> CreateClaim(Claim newClaim);

        Task UpdateClaim(Claim updatedClaim);

        Task DeleteClaim(int id);
    }
}
