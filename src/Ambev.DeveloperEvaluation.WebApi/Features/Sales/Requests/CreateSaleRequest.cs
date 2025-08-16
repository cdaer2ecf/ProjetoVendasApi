using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Requests
{
    public record CreateSaleItemRequest(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity);

    public record CreateSaleRequest(
        [Required] string Number,
        [Required] DateTime Date,
        [Required] Guid CustomerId,
        [Required] string CustomerName,
        [Required] Guid BranchId,
        [Required] string BranchName,
        List<CreateSaleItemRequest> Items);
}
