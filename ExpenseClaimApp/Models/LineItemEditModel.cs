using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class LineItemEditModel
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public int CategoryId { get; set; }
        public string Payee { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        public Decimal Amount { get; set; } //= 300.5m;
        public int CurrencyId { get; set; }
        [Required]
        public Decimal USDAmount { get; set; } //= 300.5m;

        public byte[] Receipt { get; set; }//image
    }
}
