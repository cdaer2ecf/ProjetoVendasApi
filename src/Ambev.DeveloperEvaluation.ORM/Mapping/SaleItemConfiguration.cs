// ORM/Mapping/SaleItemConfiguration.cs
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        // ORM/Mapping/SaleItemConfiguration.cs
        public void Configure(EntityTypeBuilder<SaleItem> b)
        {
            b.ToTable("saleitems");

            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("Id");

            b.Property(x => x.SaleId).HasColumnName("SaleId").IsRequired();
            b.Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();


            b.Property(x => x.ProductName)
                .HasColumnName("ProductName")
                .HasMaxLength(200)
                .IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("Quantity")
                .IsRequired();


            b.Property(x => x.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasPrecision(12, 2)
                .IsRequired();


            b.Property(x => x.DiscountPercent)
                .HasColumnName("DiscountPercent")
                .HasPrecision(5, 2)
                .IsRequired();


            b.HasOne<Sale>()
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_saleitems_sales");


            b.HasIndex(x => x.SaleId).HasDatabaseName("ix_saleitems_saleid");
            b.HasIndex(x => new { x.SaleId, x.ProductId }).HasDatabaseName("ix_saleitems_saleid_productid");

            b.Property(x => x.ProductName).HasColumnName("ProductName").HasMaxLength(200).IsRequired();
            b.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            b.Property(x => x.UnitPrice).HasColumnName("UnitPrice").HasPrecision(18, 2).IsRequired();

            b.Property(x => x.DiscountPercent)
             .HasColumnName("DiscountPercent")
             .HasPrecision(5, 2)     // garante numeric(5,2)
             .HasDefaultValue(0m)    // default 0 no banco
             .IsRequired();

             

            b.HasOne<Sale>()                             // << aqui
             .WithMany(s => s.Items)
             .HasForeignKey(si => si.SaleId)
             .OnDelete(DeleteBehavior.Cascade)
             .HasConstraintName("fk_saleitems_sales");

        }

    }
}
