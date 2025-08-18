using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
    {
        private readonly DefaultContext _db;
        private readonly IEventPublisher _eventPublisher;

        public UpdateSaleHandler(DefaultContext db, IEventPublisher eventPublisher)
        {
            _db = db;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(UpdateSaleCommand r, CancellationToken ct)
        {
            var sale = await _db.Sales.Include(s => s.Items)
                                      .FirstOrDefaultAsync(s => s.Id == r.Id, ct);
            if (sale is null) return false;

            sale.ChangeHeader(r.Date, r.CustomerId, r.CustomerName, r.BranchId, r.BranchName);
            await _db.SaveChangesAsync(ct);

            await _eventPublisher.Publish(new SaleModifiedEvent(sale.Id, DateTime.UtcNow));

            return true;
        }
    }
}
