using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly DefaultContext _db;
        public CreateSaleHandler(DefaultContext db) => _db = db;

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken ct)
        {
            var sale = new Sale(
                id: Guid.NewGuid(),
                number: request.Number,
                date: request.Date,
                customerId: request.CustomerId,
                customerName: request.CustomerName,
                branchId: request.BranchId,
                branchName: request.BranchName
            );

            if (request.Items is { Count: > 0 })
            {
                foreach (var i in request.Items)
                {
                    sale.AddItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity);
                }
            }

            await _db.Set<Sale>().AddAsync(sale, ct);
            await _db.SaveChangesAsync(ct);

            return new CreateSaleResult
            {
                Id = sale.Id,
                Number = sale.Number,
                Total = sale.Total
            };
        }
    }
}
