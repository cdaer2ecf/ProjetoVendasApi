using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{

    



    public sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> b)
        {
            b.ToTable("sales");
            b.HasKey(x => x.Id);

            // ... (demais propriedades)

            b.Property(x => x.Cancelled).HasColumnName("IsCancelled").IsRequired();

            // usar o backing field _items
            b.Metadata.FindNavigation(nameof(Sale.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

          
        }
    }


}
