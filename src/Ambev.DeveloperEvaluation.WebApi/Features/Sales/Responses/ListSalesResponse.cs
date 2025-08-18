namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Responses
{
    public record ListSalesRowResponse(Guid Id, string Number, DateTime Date, string CustomerName, string BranchName, bool Cancelled, decimal Total);
    public record ListSalesResponse(int Page, int PageSize, int TotalCount, List<ListSalesRowResponse> Items);
}
