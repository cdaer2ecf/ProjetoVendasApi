using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddItem
{
    public class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, bool>
    {
        private readonly DefaultContext _db;
        public AddSaleItemHandler(DefaultContext db) => _db = db;

        public async Task<bool> Handle(AddSaleItemCommand r, CancellationToken ct)
        {
           var sale = await _db.Sales
                                .Include(s => s.Items)
                                .FirstOrDefaultAsync(s => s.Id == r.SaleId, ct);
            if (sale is null) return false;

            var item = new SaleItem(r.ProductId, r.ProductName, r.UnitPrice, r.Quantity);
           
            _db.Entry(item).Property("SaleId").CurrentValue = sale.Id;
          
            await _db.SaleItems.AddAsync(item, ct);
                       
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
