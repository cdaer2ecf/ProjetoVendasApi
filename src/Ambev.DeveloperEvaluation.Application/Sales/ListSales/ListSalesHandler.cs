using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    public class ListSalesHandler : IRequestHandler<ListSalesQuery, ListSalesResult>
    {
        private readonly DefaultContext _db;

        public ListSalesHandler(DefaultContext db) => _db = db;

        public async Task<ListSalesResult> Handle(ListSalesQuery q, CancellationToken ct)
        {
            var query = _db.Sales.AsNoTracking().Include(s => s.Items).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Number))
                query = query.Where(s => s.Number.Contains(q.Number));

            if (!string.IsNullOrWhiteSpace(q.CustomerName))
                query = query.Where(s => s.CustomerName.Contains(q.CustomerName));

            if (q.DateFrom.HasValue)
                query = query.Where(s => s.Date >= q.DateFrom.Value);

            if (q.DateTo.HasValue)
                query = query.Where(s => s.Date <= q.DateTo.Value);

            var total = await query.CountAsync(ct);

            var skip = (q.Page - 1) * q.PageSize;
            var list = await query
                .OrderByDescending(s => s.Date)
                .Skip(skip)
                .Take(q.PageSize)
                .Select(s => new ListSalesRow(
                    s.Id, s.Number, s.Date, s.CustomerName, s.BranchName, s.Cancelled, s.Total))
                .ToListAsync(ct);

            return new ListSalesResult(q.Page, q.PageSize, total, list);
        }
    }
}
