using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eyewear.Shop.Infrastructure.Configurations;

public partial class OrderConfiguration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Authority)
                   .HasMaxLength(100);

            builder.Property(p => p.RefId)
                   .HasMaxLength(100);

            builder.Property(p => p.Gateway)
                   .HasMaxLength(50)
                   .HasDefaultValue("ZarinPal");

            builder.Property(p => p.Status)
                   .HasConversion<int>()
                   .IsRequired();

            builder.HasOne(p => p.Order)
                   .WithOne(o => o.Payment)
                   .HasForeignKey<Payment>(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Payments");
        }
    }


}
