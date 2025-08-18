using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

 
namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
   

    public sealed class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public List<CreateSaleItem> Items { get; set; } = new();
    }

    public sealed class CreateSaleItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

}
