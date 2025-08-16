using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public sealed class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
    {
        private readonly DefaultContext _db;
        public GetSaleHandler(DefaultContext db) => _db = db;

        public async Task<GetSaleResult?> Handle(GetSaleQuery request, CancellationToken ct)
        {
            var s = await _db.Sales.Include(x => x.Items)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (s is null) return null;

            var items = s.Items.Select(i =>
                new GetSaleItemResult(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.DiscountPercent, i.Total)
            ).ToList();

            return new GetSaleResult(
                s.Id, s.Number, s.Date, s.CustomerId, s.CustomerName,
                s.BranchId, s.BranchName, s.Cancelled, s.Total, items);
        }
    }
}
