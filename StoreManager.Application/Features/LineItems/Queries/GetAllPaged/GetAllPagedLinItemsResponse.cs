using System;

namespace StoreManager.Application.Features.LineItems.Queries.GetAllPaged
{
    public class GetAllPagedLinItemsResponse
    {
        public int Id { get; set; }
        public string Payee { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Decimal Amount { get; set; }
        public Decimal USDAmount { get; set; }

    }
}