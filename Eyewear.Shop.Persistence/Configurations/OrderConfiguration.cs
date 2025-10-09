using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eyewear.Shop.Infrastructure.Configurations;

public partial class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.TotalAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(o => o.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(o => o.PaymentMethod)
               .HasMaxLength(30);

        builder.Property(o => o.TrackingCode)
               .HasMaxLength(50);

        builder.HasMany(o => o.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Payment)
               .WithOne(p => p.Order)
               .HasForeignKey<Payment>(p => p.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Orders");
    }


}
