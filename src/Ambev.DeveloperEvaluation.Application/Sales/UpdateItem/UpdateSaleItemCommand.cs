using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateItem
{
    public record UpdateSaleItemCommand(Guid SaleId, Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)
        : IRequest<bool>;
}
