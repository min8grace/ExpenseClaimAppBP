using StoreManager.Domain.Entities.Expense;
using System;

namespace StoreManager.Application.Features.Claims.Queries.GetSearchClaims
{
    public class GetSearchClaimsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Requester { get; set; }
        public string Approver { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Decimal TotalAmount { get; set; } //= 300.5m;
        public Status Status { get; set; }
        public string RequesterComments { get; set; }
        public string ApproverComments { get; set; }
        public string FinanceComments { get; set; }
    }
}