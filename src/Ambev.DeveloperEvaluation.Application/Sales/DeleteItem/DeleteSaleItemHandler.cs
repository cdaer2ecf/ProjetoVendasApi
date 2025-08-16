using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteItem
{
    public class DeleteSaleItemHandler : IRequestHandler<DeleteSaleItemCommand, bool>
    {
        private readonly DefaultContext _db;
        public DeleteSaleItemHandler(DefaultContext db) => _db = db;

        public async Task<bool> Handle(DeleteSaleItemCommand r, CancellationToken ct)
        {
            var sale = await _db.Sales.Include(s => s.Items)
                                      .FirstOrDefaultAsync(s => s.Id == r.SaleId, ct);
            if (sale is null) return false;

            sale.CancelItem(r.ProductId);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
