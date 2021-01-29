using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Domain.Entities.Expense
{
    public class Currency
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public ICollection<LineItem> LineItems { get; set; }
    }
}
