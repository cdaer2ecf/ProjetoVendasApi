using Ambev.DeveloperEvaluation.Domain;
using Ambev.DeveloperEvaluation.Domain.Sales.Policies;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Policies;

public class DiscountPolicyTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Should_apply_expected_discount(int qty, decimal expected)
        => DiscountPolicy.ForQuantity(qty).Should().Be(expected);

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_throw_when_qty_not_positive(int qty)
    {
        Action act = () => DiscountPolicy.ForQuantity(qty);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_throw_when_qty_gt_20()
    {
        Action act = () => DiscountPolicy.ForQuantity(21);
        act.Should().Throw<DomainException>();
    }

}
