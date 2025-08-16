using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public interface IDomainEvent { }

    public sealed record SaleCreated(Guid SaleId) : IDomainEvent;
    public sealed record SaleModified(Guid SaleId) : IDomainEvent;
    public sealed record SaleCancelled(Guid SaleId) : IDomainEvent;
    public sealed record ItemCancelled(Guid SaleId, Guid ProductId) : IDomainEvent;
}
