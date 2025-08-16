using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateItem
{
    public class UpdateSaleItemHandler : IRequestHandler<UpdateSaleItemCommand, bool>
    {
        private readonly DefaultContext _db;
        public UpdateSaleItemHandler(DefaultContext db) => _db = db;

        public async Task<bool> Handle(UpdateSaleItemCommand r, CancellationToken ct)
        {
            var sale = await _db.Sales.Include(s => s.Items)
                                      .FirstOrDefaultAsync(s => s.Id == r.SaleId, ct);
            if (sale is null) return false;

            sale.UpdateItem(r.ProductId, r.ProductName, r.UnitPrice, r.Quantity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
