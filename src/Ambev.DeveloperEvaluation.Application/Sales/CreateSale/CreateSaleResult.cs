namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public sealed class CreateSaleResult
    {
        public Guid Id { get; init; }
        public string Number { get; init; } = string.Empty;
        public decimal Total { get; init; }
    }
}
