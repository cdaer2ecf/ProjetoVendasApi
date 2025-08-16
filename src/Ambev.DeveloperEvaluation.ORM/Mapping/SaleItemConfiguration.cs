using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using global::Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    

   

    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> b)
        {
            b.ToTable("saleitems");
            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("Id");
            b.Property(x => x.SaleId).HasColumnName("SaleId").IsRequired();
            b.Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            b.Property(x => x.ProductName).HasColumnName("ProductName").HasMaxLength(200).IsRequired();
            b.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            b.Property(x => x.UnitPrice).HasColumnName("UnitPrice").HasPrecision(18, 2).IsRequired();
            b.Property(x => x.DiscountPercent).HasColumnName("DiscountPercent").HasPrecision(5, 4);

            
            b.HasOne(si => si.Sale)
             .WithMany(s => s.Items)
             .HasForeignKey(si => si.SaleId)
             .OnDelete(DeleteBehavior.Cascade)
             .HasConstraintName("fk_saleitems_sales");
        }
    }

}
