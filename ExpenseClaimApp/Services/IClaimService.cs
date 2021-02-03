using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
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

        Task<Claim> GetClaimById(int id);

        Task<Claim> CreateClaim(Claim newClaim);

        Task UpdateClaim(Claim updatedClaim);

        Task DeleteClaim(int id);
    }
}
