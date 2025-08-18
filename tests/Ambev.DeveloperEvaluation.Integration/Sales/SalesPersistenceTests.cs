using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Db;
using Ambev.DeveloperEvaluation.ORM;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales;

public class SalesPersistenceTests
{
    [Fact]
    public async Task Create_Add_Update_Delete_Cancel_persiste_corretamente()
    {
        await using var db = ContextFactory.New();

        var sale = new Sale(Guid.NewGuid(), "S-INT-1", DateTime.UtcNow,
            Guid.NewGuid(), "Cliente", Guid.NewGuid(), "Filial");

        // ADD ITEM ANTES DE SALVAR A VENDA
        sale.AddItem(Guid.NewGuid(), "Prod", 100, 4); // 360

        db.Sales.Add(sale);
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        var rec = await db.Sales.Include(s => s.Items).FirstAsync(s => s.Id == sale.Id);
        rec.Total.Should().Be(360);

        var pid = rec.Items.First().ProductId;
        rec.UpdateItem(pid, "Prod", 100, 10); // 800
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        var total = await db.Sales.Include(s => s.Items)
                                  .Where(s => s.Id == sale.Id)
                                  .Select(s => s.Total)
                                  .FirstAsync();
        total.Should().Be(800);

        var rec2 = await db.Sales.FirstAsync(s => s.Id == sale.Id);
        rec2.Cancel();
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        (await db.Sales.Where(s => s.Id == sale.Id)
                       .Select(s => s.Cancelled).FirstAsync()).Should().BeTrue();
    }

}
