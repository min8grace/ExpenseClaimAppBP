using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Infrastructure.CacheKeys
{
    public static class LineItemCacheKeys
    {
        public static string ListKey => "LineItemList";

        public static string SelectListKey => "LineItemSelectList";

        public static string GetKey(int lineItemId) => $"LineItem-{lineItemId}";

        public static string GetDetailsKey(int lineItemId) => $"LineItemDetails-{lineItemId}";
    }
}
