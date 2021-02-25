using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Domain.Entities.Expense
{

    public enum Status
    {
        Requested = 1, Approved = 2, Rejected = 3, Queried = 4, Processing = 5, RejectedByFinance = 7, Finished = 8, Cancel = 9, Saved = 99
    }
    public class Claim : AuditableEntity
    {
        [Required(ErrorMessage = "Custom Validation : Title must be provided")]
        [MinLength(5)]
        public string Title { get; set; }
        public string Requester { get; set; }
        public string Approver { get; set; }
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

        public virtual ICollection<LineItem> LineItems { get; set; }
    }
}
