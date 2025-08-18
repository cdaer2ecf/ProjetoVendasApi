using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleModifiedEvent : INotification
    {
        public Guid SaleId { get; }
        public DateTime ModifiedAt { get; }

        public SaleModifiedEvent(Guid saleId, DateTime modifiedAt)
        {
            SaleId = saleId;
            ModifiedAt = modifiedAt;
        }
    }
}

