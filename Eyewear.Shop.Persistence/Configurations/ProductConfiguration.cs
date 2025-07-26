using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eyewear.Shop.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);

        builder.HasMany(p => p.Variants)
               .WithOne(v => v.Product)
               .HasForeignKey(v => v.ProductId);

        builder.HasMany(p => p.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId);
    }
}
