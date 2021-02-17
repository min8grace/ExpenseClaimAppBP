using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Application.Interfaces.Repositories
{
    public interface IClaimRepository
    {
        IQueryable<Claim> Claims { get; }

        Task<List<Claim>> GetListAsync();
        Task<List<Claim>> GetSearchClaimsAsync(string searchString);
        
        Task<Claim> GetByIdAsync(int claimId);

        Task<int> InsertAsync(Claim claim);

        Task UpdateAsync(Claim claim);

        Task DeleteAsync(Claim claim);
    }
}
