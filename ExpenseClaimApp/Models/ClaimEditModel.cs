using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class ClaimEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Custom Validation : Title must be provided")]
        [MinLength(5)]
        public string Title { get; set; }
        public int Requester { get; set; }
        public int Approver { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SubmitDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public Decimal TotalAmount { get; set; } //= 300.5m;
        public Status Status { get; set; }
        public string RequesterComments { get; set; }
        public string ApproverComments { get; set; }
        public string FinanceComments { get; set; }

        [ValidateComplexType]
        public virtual ICollection<LineItem> LineItems { get; set; } = new Collection<LineItem>();
    }
}
