// Domain/Entities/SaleItem.cs
using Ambev.DeveloperEvaluation.Domain.Sales.Policies;

public sealed class SaleItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    // ⬇️ NÃO persista. Calcule sempre:
   // public decimal DiscountPercent => DiscountPolicy.ForQuantity(Quantity);
    public decimal DiscountPercent { get; private set; }

    // Total sempre correto:
    public decimal Total => Math.Round(Quantity * UnitPrice * (1 - DiscountPercent / 100m), 2);

    private SaleItem() { } // EF

    public SaleItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty) throw new DomainException("ProductId is required");
        if (string.IsNullOrWhiteSpace(productName)) throw new DomainException("ProductName is required");
        if (unitPrice < 0) throw new DomainException("UnitPrice cannot be negative");

        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }
    public void SetPrice(decimal unitPrice)
    {
        if (unitPrice < 0) throw new DomainException("UnitPrice cannot be negative");
        UnitPrice = unitPrice;
    }
    public void SetQuantity(int quantity)
    {
        // valida faixas via policy; DiscountPercent agora é derivado
        DiscountPercent = DiscountPolicy.ForQuantity(quantity);
        Quantity = quantity;
    }

    public void RenameProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("ProductName is required");
        ProductName = name.Trim();
    }
}
