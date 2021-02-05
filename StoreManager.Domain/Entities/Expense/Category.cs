using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Domain.Entities.Expense
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        //public ICollection<LineItem> LineItems { get; set; }
    }
}
