using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress);

            builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.DeliveryMethod).WithMany().HasForeignKey(o => o.DeliveryMethodId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.SubTotal).HasColumnType("decimal(8, 3)");

            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        }
    }
}
