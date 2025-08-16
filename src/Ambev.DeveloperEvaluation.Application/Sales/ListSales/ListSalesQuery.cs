using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public record ListSalesQuery(
        string? Number,
        string? CustomerName,
        DateTime? DateFrom,
        DateTime? DateTo,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<ListSalesResult>;

    public record ListSalesRow(Guid Id, string Number, DateTime Date, string CustomerName, string BranchName, bool Cancelled, decimal Total);

    public record ListSalesResult(int Page, int PageSize, int TotalCount, List<ListSalesRow> Items);
}
