using System;

namespace StoreManager.Application.Features.LineItems.Queries.GetAllPaged
{
    public class GetAllLineItemsResponse
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

    }
}