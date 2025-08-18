using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteItem
{
    public class DeleteSaleItemHandler : IRequestHandler<DeleteSaleItemCommand, bool>
    {
        private readonly DefaultContext _db;
        private readonly IEventPublisher _eventPublisher;

        public DeleteSaleItemHandler(DefaultContext db, IEventPublisher eventPublisher)
        {
            _db = db;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(DeleteSaleItemCommand r, CancellationToken ct)
        {
            var sale = await _db.Sales.Include(s => s.Items)
                                      .FirstOrDefaultAsync(s => s.Id == r.SaleId, ct);
            if (sale is null) return false;

            sale.CancelItem(r.ProductId);
            await _db.SaveChangesAsync(ct);

            await _eventPublisher.Publish(new ItemCancelledEvent(r.SaleId, r.ProductId, DateTime.UtcNow));

            return true;
        }
    }
}
