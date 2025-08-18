using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddItem
{
    public record AddSaleItemCommand(Guid SaleId, Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)
        : IRequest<bool>;
}
