namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
   

    public sealed class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
    }

}
