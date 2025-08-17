using Ambev.DeveloperEvaluation.Domain;                    // DomainException
using Ambev.DeveloperEvaluation.Domain.Sales.Policies;     // DiscountPolicy
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Policies;

public class DiscountPolicyTests
{
    // Faixas e limites exatos
    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Should_apply_expected_discount(int quantity, decimal expected)
    {
        DiscountPolicy.ForQuantity(quantity).Should().Be(expected);
    }

    // Quantidades inválidas
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_throw_when_qty_not_positive(int quantity)
    {
        Action act = () => DiscountPolicy.ForQuantity(quantity);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_throw_when_qty_greater_than_20()
    {
        Action act = () => DiscountPolicy.ForQuantity(21);
        act.Should().Throw<DomainException>();
    }
}
