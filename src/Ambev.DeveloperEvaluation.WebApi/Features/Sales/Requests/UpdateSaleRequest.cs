namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Requests
{
    public record UpdateSaleRequest(
        DateTime Date,
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName);
}
