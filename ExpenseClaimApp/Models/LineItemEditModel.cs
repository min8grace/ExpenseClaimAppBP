using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class LineItemEditModel
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        [Required(ErrorMessage = "Categor must be provided")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Payee must be provided")]
        public string Payee { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Description must be provided")]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public Decimal Amount { get; set; } 
        public int CurrencyId { get; set; }
        [Required]
        public Decimal USDAmount { get; set; } 

        public byte[] Receipt { get; set; }
    }
}
