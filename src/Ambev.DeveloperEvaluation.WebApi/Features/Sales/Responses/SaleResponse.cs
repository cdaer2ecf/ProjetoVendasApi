namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses
{
    public record SaleItemResponse(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal DiscountPercent, decimal Total);

    public record SaleResponse(
        Guid Id,
        string Number,
        DateTime Date,
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName,
        bool Cancelled,
        decimal Total,
        List<SaleItemResponse> Items);
}
