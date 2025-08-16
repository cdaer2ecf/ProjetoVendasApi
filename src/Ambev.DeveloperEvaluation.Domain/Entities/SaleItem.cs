using System;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public sealed class SaleItem
    {
        // ✅ chaves/relacionamento (existem no banco)
        public Guid Id { get; private set; }          // PK
        public Guid SaleId { get; private set; }      // FK -> sales.id
        public Sale Sale { get; private set; } = null!; // navegação para funcionar i => i.Sale

        // ✅ colunas de item (existem no banco)
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal DiscountPercent { get; private set; } // 0..100 (%)

        // Total = qty * price * (1 - %/100)
        public decimal Total => Math.Round(Quantity * UnitPrice * (1 - DiscountPercent / 100m), 2);

        private SaleItem() { } // EF

        public SaleItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            if (productId == Guid.Empty) throw new DomainException("ProductId is required");
            if (string.IsNullOrWhiteSpace(productName)) throw new DomainException("ProductName is required");
            if (unitPrice < 0) throw new DomainException("UnitPrice cannot be negative");
            if (quantity <= 0) throw new DomainException("Quantity must be greater than zero");

            Id = Guid.NewGuid(); // ✅ gera PK
            ProductId = productId;
            ProductName = productName.Trim();
            SetPrice(unitPrice);
            SetQuantity(quantity); // ajusta DiscountPercent pela policy (se você usar)
        }

        public void SetPrice(decimal unitPrice)
        {
            if (unitPrice < 0) throw new DomainException("UnitPrice cannot be negative");
            UnitPrice = unitPrice;
        }

        public void SetQuantity(int quantity)
        {
            // Se você usa a DiscountPolicy:
            // DiscountPercent = DiscountPolicy.ForQuantity(quantity);
            if (quantity <= 0) throw new DomainException("Quantity must be greater than zero");
            Quantity = quantity;
        }

        public void SetDiscount(decimal pct)
        {
            if (pct < 0 || pct > 100) throw new DomainException("Discount must be between 0 and 100");
            DiscountPercent = pct;
        }

        public void RenameProduct(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("ProductName is required");
            ProductName = name.Trim();
        }
    }
}
