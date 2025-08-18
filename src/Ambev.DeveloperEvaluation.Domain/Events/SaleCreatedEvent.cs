using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : INotification
    {
        public Guid SaleId { get; }
        public DateTime CreatedAt { get; }

        public SaleCreatedEvent(Guid saleId, DateTime createdAt)
        {
            SaleId = saleId;
            CreatedAt = createdAt;
        }
    }
}

