using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Infrastructure.CacheKeys
{
    public static class CategoryCacheKeys
    {
        public static string ListKey => "CategoryList";

        public static string SelectListKey => "CategorySelectList";

        public static string GetKey(int categoryId) => $"Category-{categoryId}";

        public static string GetDetailsKey(int categoryId) => $"CategoryDetails-{categoryId}";
    }
}
