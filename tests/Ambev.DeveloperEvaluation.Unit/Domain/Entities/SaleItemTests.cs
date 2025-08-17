using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Fact]
    public void Qty3_Total_sem_desconto()
    {
        var i = new SaleItem(Guid.NewGuid(), "Prod", 100m, 3);
        i.DiscountPercent.Should().Be(0);
        i.Total.Should().Be(300m);
    }

    [Fact]
    public void Qty4_Total_com_10pct()
    {
        var i = new SaleItem(Guid.NewGuid(), "Prod", 100m, 4);
        i.DiscountPercent.Should().Be(10);
        i.Total.Should().Be(360m);
    }

    [Fact]
    public void Qty10_Total_com_20pct()
    {
        var i = new SaleItem(Guid.NewGuid(), "Prod", 100m, 10);
        i.DiscountPercent.Should().Be(20);
        i.Total.Should().Be(800m);
    }

    [Fact]
    public void Qty21_Dispara_excecao()
    {
        Action act = () => new SaleItem(Guid.NewGuid(), "Prod", 100m, 21);
        act.Should().Throw<DomainException>();
    }
}
