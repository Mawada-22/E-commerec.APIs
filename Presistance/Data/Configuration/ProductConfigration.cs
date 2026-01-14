using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Presistance.Data.Configuration
{
    public class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.productBrand).WithMany().HasForeignKey(P => P.ProductBrandId);

            builder.HasOne(P=>P.productType).WithMany().HasForeignKey(p => p.ProductTypeId);
           
            builder.Property(P => P.Price).HasColumnType("decimal(8, 3)");
        }
    }
}
