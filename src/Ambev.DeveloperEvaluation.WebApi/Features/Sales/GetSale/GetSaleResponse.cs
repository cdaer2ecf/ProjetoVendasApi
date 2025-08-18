namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{


    public sealed class GetSaleResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public bool Cancelled { get; set; }
        public decimal Total { get; set; }
        public List<GetSaleItemResponse> Items { get; set; } = new();
    }

    public sealed class GetSaleItemResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal Total { get; set; }
    }

}
