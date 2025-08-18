using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public sealed class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly DefaultContext _db;
        private readonly IEventPublisher _eventPublisher;

        public CancelSaleHandler(DefaultContext db, IEventPublisher eventPublisher)
        {
            _db = db;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken ct)
        {
            var sale = await _db.Sales
                                .Include(s => s.Items)
                                .FirstOrDefaultAsync(s => s.Id == request.Id, ct);
            if (sale is null) return false;

            sale.Cancel();
            await _db.SaveChangesAsync(ct);

            await _eventPublisher.Publish(new SaleCancelledEvent(sale.Id, DateTime.UtcNow));

            return true;
        }
    }
}
