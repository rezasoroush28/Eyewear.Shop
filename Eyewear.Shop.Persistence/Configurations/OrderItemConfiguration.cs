using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eyewear.Shop.Infrastructure.Configurations;

public partial class OrderConfiguration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(i => i.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Quantity)
                   .IsRequired();

            builder.ToTable("OrderItems");
        }
    }


}
