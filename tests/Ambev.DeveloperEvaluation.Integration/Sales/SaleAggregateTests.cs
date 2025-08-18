using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Db;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales;

public class SaleAggregateTests
{

    [Fact]
    public async Task Create_Add_Update_Delete_Cancel_persiste_corretamente()
    {
        await using var db = ContextFactory.New();

        var sale = new Sale(Guid.NewGuid(), "S-INT-1", DateTime.UtcNow,
            Guid.NewGuid(), "Cliente", Guid.NewGuid(), "Filial");

        // cria item com qty=4 -> 10%
        sale.AddItem(Guid.NewGuid(), "Prod", 100, 4);
        db.Sales.Add(sale);

        await db.SaveChangesAsync();

        // força recarga do store (evita pegar a mesma instância rastreada)
        db.ChangeTracker.Clear();

        // ---- verificação pós-inserção ----
        var rec = await db.Sales
                          .Include(s => s.Items)
                          .FirstAsync(s => s.Id == sale.Id);

        rec.Items.Should().HaveCount(1);
        var it = rec.Items.First();
        it.Quantity.Should().Be(4);
        it.UnitPrice.Should().Be(100);
        it.DiscountPercent.Should().Be(10);   // << confirmar desconto
        rec.Total.Should().Be(360);           // 4 * 100 * 0.9

        // ---- update para qty=10 -> 20% ----
        var pid = it.ProductId;
        rec.UpdateItem(pid, "Prod", 100, 10);
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        var totalAposUpdate = await db.Sales
                                      .Include(s => s.Items)
                                      .Where(s => s.Id == sale.Id)
                                      .Select(s => new
                                      {
                                          Total = s.Total,
                                          Item = s.Items.First()
                                      })
                                      .FirstAsync();

        totalAposUpdate.Item.DiscountPercent.Should().Be(20);
        totalAposUpdate.Total.Should().Be(800); // 10 * 100 * 0.8

        // ---- cancelamento ----
        var rec2 = await db.Sales.FirstAsync(s => s.Id == sale.Id);
        rec2.Cancel();
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        (await db.Sales.Where(s => s.Id == sale.Id)
                       .Select(s => s.Cancelled)
                       .FirstAsync()).Should().BeTrue();
    }


}
