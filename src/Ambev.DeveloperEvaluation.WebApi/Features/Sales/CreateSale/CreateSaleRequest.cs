namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
   
    public sealed class CreateSaleRequest
    {
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public List<CreateSaleItemRequest> Items { get; set; } = new();
    }

    public sealed class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

}
