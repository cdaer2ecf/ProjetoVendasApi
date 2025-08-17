using Ambev.DeveloperEvaluation.Domain;                 // DomainException
using Ambev.DeveloperEvaluation.Domain.Entities;        // Sale
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleAggregateTests
{
    [Fact]
    public void Add_two_items_and_total_ok()
    {
        var sale = NovaSale();

        var pidA = Guid.NewGuid();
        var pidB = Guid.NewGuid();

        sale.AddItem(pidA, "A", 100m, 4);  // 360 (10%)
        sale.AddItem(pidB, "B", 50m, 3);  // 150 (0%)

        sale.Total.Should().Be(510m);
        sale.Items.Should().HaveCount(2);
    }

    [Fact]
    public void Update_item_recalculates_total()
    {
        var sale = NovaSale();

        var pidA = Guid.NewGuid();
        var pidB = Guid.NewGuid();

        sale.AddItem(pidA, "A", 100m, 4);  // 360
        sale.AddItem(pidB, "B", 50m, 3);  // 150
        sale.Total.Should().Be(510m);

        // Atualiza B para 10 unidades (20% desc): 10 * 50 * 0.8 = 400
        sale.UpdateItem(pidB, "B", 50m, 10);

        sale.Total.Should().Be(360m + 400m); // 760
    }

    [Fact]
    public void Cancel_item_recalculates_total()
    {
        var sale = NovaSale();

        var pidA = Guid.NewGuid();
        var pidB = Guid.NewGuid();

        sale.AddItem(pidA, "A", 100m, 4);  // 360
        sale.AddItem(pidB, "B", 50m, 3);  // 150
        sale.Total.Should().Be(510m);

        sale.CancelItem(pidA);

        sale.Items.Should().HaveCount(1);
        sale.Total.Should().Be(150m);
    }

    [Fact]
    public void Cancel_sale_sets_flag_and_blocks_changes()
    {
        var sale = NovaSale();

        var pid = Guid.NewGuid();
        sale.AddItem(pid, "A", 100m, 4);   // 360

        sale.Cancel();
        sale.Cancelled.Should().BeTrue();

        // qualquer alteração após cancelamento deve falhar
        Action add = () => sale.AddItem(Guid.NewGuid(), "X", 10m, 1);
        Action update = () => sale.UpdateItem(pid, "A", 100m, 5);
        Action remove = () => sale.CancelItem(pid);

        add.Should().Throw<DomainException>();
        update.Should().Throw<DomainException>();
        remove.Should().Throw<DomainException>();
    }

    private static Sale NovaSale() =>
        new Sale(Guid.NewGuid(), "S-UNIT-1", DateTime.UtcNow,
                 Guid.NewGuid(), "Cliente", Guid.NewGuid(), "Filial");
}
