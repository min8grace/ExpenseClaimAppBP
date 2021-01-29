using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Infrastructure.CacheKeys
{
    public static class ClaimCacheKeys
    {
        public static string ListKey => "ClaimList";

        public static string SelectListKey => "ClaimSelectList";

        public static string GetKey(int claimId) => $"Claim-{claimId}";

        public static string GetDetailsKey(int claimId) => $"ClaimDetails-{claimId}";
    }
}
