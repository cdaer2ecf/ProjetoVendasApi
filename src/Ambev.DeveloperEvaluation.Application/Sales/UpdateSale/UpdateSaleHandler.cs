using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
    {
        private readonly DefaultContext _db;
        public UpdateSaleHandler(DefaultContext db) => _db = db;

        public async Task<bool> Handle(UpdateSaleCommand r, CancellationToken ct)
        {
            var sale = await _db.Sales.Include(s => s.Items)
                                      .FirstOrDefaultAsync(s => s.Id == r.Id, ct);
            if (sale is null) return false;

            sale.ChangeHeader(r.Date, r.CustomerId, r.CustomerName, r.BranchId, r.BranchName);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
