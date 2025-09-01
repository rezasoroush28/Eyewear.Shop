using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eyewear.Shop.Persistence.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.VarientPrice)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variants)
               .HasForeignKey(v => v.ProductId);

        // Variant attributes (key–value pairs)
        builder.OwnsMany(v => v.VarientAttributes, a =>
        {
            a.WithOwner().HasForeignKey("ProductVariantId");
            a.Property(p => p.Key).HasMaxLength(100).IsRequired();
            a.Property(p => p.Value).HasMaxLength(200).IsRequired();
            a.ToTable("ProductVariantAttributes");
        });
    }
}
