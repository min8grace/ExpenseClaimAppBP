using StoreManager.Domain.Entities.Expense;
using System;

namespace StoreManager.Application.Features.LineItems.Queries.GetById
{
    public class GetLineItemByIdResponse
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public int CategoryId { get; set; }
        public string Payee { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Decimal Amount { get; set; } 
        public string CurrencyCode { get; set; }
        public Decimal USDAmount { get; set; } 

        public byte[] Receipt { get; set; }//image

    }
}