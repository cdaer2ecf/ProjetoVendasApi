using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using global::Ambev.DeveloperEvaluation.Domain.Events;

 
namespace Ambev.DeveloperEvaluation.Domain.Entities
{
   

    public sealed class Sale
    {
        public Guid Id { get; private set; }
        public string Number { get; private set; } = string.Empty;
        public DateTime Date { get; private set; }

        // Cliente (denormalizado)
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;

        // Filial (denormalizado)
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; } = string.Empty;

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        public bool Cancelled { get; private set; }

        /// <summary>Total da venda = soma dos totais dos itens (2 casas).</summary>
        public decimal Total => Math.Round(_items.Sum(i => i.Total), 2);

        // Eventos de domínio capturados durante o ciclo de vida da aggregate
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private Sale() { } // EF

        public Sale(
            Guid id,
            string number,
            DateTime date,
            Guid customerId,
            string customerName,
            Guid branchId,
            string branchName)
        {
            if (id == Guid.Empty) throw new DomainException("Id is required");
            if (string.IsNullOrWhiteSpace(number)) throw new DomainException("Number is required");
            if (customerId == Guid.Empty) throw new DomainException("CustomerId is required");
            if (string.IsNullOrWhiteSpace(customerName)) throw new DomainException("CustomerName is required");
            if (branchId == Guid.Empty) throw new DomainException("BranchId is required");
            if (string.IsNullOrWhiteSpace(branchName)) throw new DomainException("BranchName is required");

            Id = id;
            Number = number.Trim();
            Date = date;
            CustomerId = customerId;
            CustomerName = customerName.Trim();
            BranchId = branchId;
            BranchName = branchName.Trim();

            Raise(new SaleCreated(Id));
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            EnsureNotCancelled();

            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing is not null)
            {
                // regra de negócio: podemos somar quantidades ou criar item separado?
                // Para simplificar, vamos substituir (update) a linha pelo novo estado
                UpdateItem(productId, productName, unitPrice, quantity);
                return;
            }

            var item = new SaleItem(productId, productName, unitPrice, quantity);
            _items.Add(item);
            Raise(new SaleModified(Id));
        }

        public void UpdateItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            EnsureNotCancelled();

            var item = _items.FirstOrDefault(i => i.ProductId == productId)
                       ?? throw new DomainException("Item not found for product");

            item.RenameProduct(productName);
            item.SetPrice(unitPrice);
            item.SetQuantity(quantity);

            Raise(new SaleModified(Id));
        }

        public void CancelItem(Guid productId)
        {
            EnsureNotCancelled();

            var item = _items.FirstOrDefault(i => i.ProductId == productId)
                       ?? throw new DomainException("Item not found for product");

            _items.Remove(item);
            Raise(new ItemCancelled(Id, productId));
            Raise(new SaleModified(Id));
        }

        public void Cancel()
        {
            if (Cancelled) return;
            Cancelled = true;
            Raise(new SaleCancelled(Id));
        }

        public void ChangeHeader(DateTime date, Guid customerId, string customerName, Guid branchId, string branchName)
        {
            EnsureNotCancelled();

            if (customerId == Guid.Empty) throw new DomainException("CustomerId is required");
            if (string.IsNullOrWhiteSpace(customerName)) throw new DomainException("CustomerName is required");
            if (branchId == Guid.Empty) throw new DomainException("BranchId is required");
            if (string.IsNullOrWhiteSpace(branchName)) throw new DomainException("BranchName is required");

            Date = date;
            CustomerId = customerId;
            CustomerName = customerName.Trim();
            BranchId = branchId;
            BranchName = branchName.Trim();

            Raise(new SaleModified(Id));
        }

        private void EnsureNotCancelled()
        {
            if (Cancelled) throw new DomainException("Sale is cancelled");
        }

        private void Raise(IDomainEvent @event) => _domainEvents.Add(@event);

        /// <summary>
        /// Limpa a lista de eventos após a publicação (chame no Application/Infra).
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();
    }

}
