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

        Task<Claim> GetByIdAsync(int brandId);

        Task<int> InsertAsync(Claim brand);

        Task UpdateAsync(Claim brand);

        Task DeleteAsync(Claim brand);
    }
}
