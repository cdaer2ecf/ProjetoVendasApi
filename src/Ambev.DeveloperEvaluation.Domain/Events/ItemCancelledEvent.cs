using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ItemCancelledEvent : INotification
    {
        public Guid SaleId { get; }
        public Guid ProductId { get; }
        public DateTime CancelledAt { get; }

        public ItemCancelledEvent(Guid saleId, Guid productId, DateTime cancelledAt)
        {
            SaleId = saleId;
            ProductId = productId;
            CancelledAt = cancelledAt;
        }
    }
}

