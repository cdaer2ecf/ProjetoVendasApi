using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent : INotification
    {
        public Guid SaleId { get; }
        public DateTime CancelledAt { get; }

        public SaleCancelledEvent(Guid saleId, DateTime cancelledAt)
        {
            SaleId = saleId;
            CancelledAt = cancelledAt;
        }
    }
}

