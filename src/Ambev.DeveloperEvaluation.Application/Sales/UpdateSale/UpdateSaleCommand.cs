using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public record UpdateSaleCommand(
        Guid Id,
        DateTime Date,
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName
    ) : IRequest<bool>;
}
