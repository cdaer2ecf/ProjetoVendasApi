namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Requests
{
    public record AddOrUpdateSaleItemRequest(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity);
}
