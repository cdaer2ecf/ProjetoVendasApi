using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public sealed class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly DefaultContext _db;
        public CancelSaleHandler(DefaultContext db) => _db = db;

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken ct)
        {
            var sale = await _db.Sales
                                .Include(s => s.Items)
                                .FirstOrDefaultAsync(s => s.Id == request.Id, ct);
            if (sale is null) return false;

            sale.Cancel();
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
