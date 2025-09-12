using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Eyewear.Shop.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);

        builder.HasMany(p => p.Variants)
               .WithOne(v => v.Product)
               .HasForeignKey(v => v.ProductId);

        builder.Property(p => p.ThumbnailImageUrl)
            .HasMaxLength(500);

        // Store List<string> as JSON in DB
        builder.Property(p => p.MainImagesUrls)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.BasePrice)
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.DiscountAmount)
               .HasColumnType("decimal(9,2)");

        builder.Property(p => p.DiscountTyp) // ✅ fix typo here
               .HasConversion<string>();

        builder.OwnsMany(p => p.Attributes, a =>
        {
            a.WithOwner().HasForeignKey("ProductId");
            a.Property(x => x.Key).HasMaxLength(100).IsRequired();
            a.Property(x => x.Value).HasMaxLength(200).IsRequired();
            a.ToTable("ProductAttributes");
        });
    }
}
