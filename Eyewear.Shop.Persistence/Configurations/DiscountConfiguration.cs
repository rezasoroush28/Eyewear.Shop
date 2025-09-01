//using Eyewear.Shop.Domain.Entities;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.EntityFrameworkCore;

//public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
//{
//    public void Configure(EntityTypeBuilder<Discount> builder)
//    {
//        builder.HasKey(d => d.Id);

//        builder.Property(d => d.Value)
//               .HasPrecision(10, 2); // Optional: better for money precision

//        builder.Property(d => d.Type)
//               .HasConversion<string>()           // Store as "Percentage" or "FixedAmount"
//               .HasMaxLength(20);                 // Optional

//        builder.Property(d => d.Description)
//               .HasMaxLength(200); // Optional
//    }
//}
