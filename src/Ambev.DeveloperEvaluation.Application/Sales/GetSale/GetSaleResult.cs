using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{


    public record GetSaleResult(
       Guid Id,
       string Number,
       DateTime Date,
       Guid CustomerId,
       string CustomerName,
       Guid BranchId,
       string BranchName,
       bool Cancelled,
       decimal Total,
       List<GetSaleItemResult> Items
   );

    public record GetSaleItemResult(
         Guid ProductId,
         string ProductName,
         decimal UnitPrice,
         int Quantity,
         decimal DiscountPercent,
         decimal Total
     );

}
