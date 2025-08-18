using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteItem
{
    public record DeleteSaleItemCommand(Guid SaleId, Guid ProductId) : IRequest<bool>;
}
